module api.App

open System
open System.IO
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Database
open FSharp.Control.Tasks.ContextInsensitive

// ---------------------------------
// Models
// ---------------------------------

type Circuit = {
    circuitId : int
    circuitRef : string
    name : string
    location : string
    country : string
    lat: single
    lng : single
    alt : int
    url : string
}

type ConstructorResult = {
    constructorResultsId : int
    raceId : int
    constructorId : int
    points : single
    status : string
}

type ConstructorStanding = {
    constructorStandingsId : int
    raceId : int
    constructorId : int
    points : single
    position : int
    positionText : string
    wins : int
}

type Constructor = {
    constructorId : int
    constructorRef : string
    name : string
    nationality : string
    url : string
}

type DriverStanding = {
    driverStandingId : int
    raceId : int
    driverId : int
    points : single
    position : int
    positionText : string
    wins : int
}

type Driver = {
    driverId : int
    driverRef : string
    number : int
    code : string
    forename : string
    surname : string
    dob : DateTimeOffset
    nationality : string
    url : string
}

type LapTime = {
    raceId : int
    driverId : int
    lap : int
    position : int
    time : string
    milliseconds : int
}

type PitStop = {
    raceId : int
    driverId : int
    stop : int
    lap : int
    time : DateTimeOffset
    duration : string
    milliseconds : int
}

type Qualifying = {
    qualifyId : int
    raceId : int
    driverId : int
    constructorId : int
    number : int
    position : int
    q1 : string
    q2 : string
    q3 : string
}

type Race = {
    raceId : int
    year : int
    round: int
    circuitId : int
    name : string
    date : DateTimeOffset
    time: DateTimeOffset
    url : string
}

type Result = {
    resultId : int
    raceId : int
    driverId : int
    constructorId : int
    number : int
    grid : int
    position : int
    positionText : string
    positionOrder : int
    points : single
    laps : int
    time : string
    milliseconds : int
    fastestLap : int
    rank : int
    fastestLapTime : string
    fastestLapSpeed : string
    statusId : int
}

type Season = {
    year : int
    url : string
}

type Status = {
    statusId : int
    status : string
}

// ---------------------------------
// Web app
// ---------------------------------

let getLaps =
    fun (next) (ctx : HttpContext) ->
        task {
            let connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            let connection = createConnection connectionString

            let query = "SELECT * FROM lapTimes;"
            let! result =
                connection
                |> dapperQuery<LapTime> query

            return! json result next ctx
        }

let getCircuits =
    fun (next) (ctx : HttpContext) ->
        task {
            let connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
            let connection = createConnection connectionString

            let query = "SELECT * FROM circuits;"
            let! result =
                connection
                |> dapperQuery<Circuit> query

            return! json result next ctx
        }

let webApp =
    choose [
        GET >=>
            choose [
                route "/circuits" >=> getCircuits
                route "/laps" >=> getLaps
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins("http://localhost:8080")
        .AllowAnyMethod()
        .AllowAnyHeader()
        |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0
