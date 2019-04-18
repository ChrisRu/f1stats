module api.DatabaseModels
open System

type Circuit = {
    circuitId : int
    circuitRef : string
    name : string
    location : string
    country : string
    lat : single
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
    round : int
    circuitId : int
    name : string
    date : DateTimeOffset
    time : DateTimeOffset
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
