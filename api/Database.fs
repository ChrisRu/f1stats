module api.Database
    open System.Threading.Tasks
    open System
    open System.Collections.Generic
    open Dapper
    open MySql.Data.MySqlClient

    let private dapperMapParametrizedQuery<'T> (connection: MySqlConnection) sql (parameters : IDictionary<string, obj> option) : Task<IEnumerable<'T>> =
        match parameters with
        | Some(p) -> connection.QueryAsync<'T>(sql, p)
        | None -> connection.QueryAsync<'T> sql


    let private createConnection connectionString =
        if String.IsNullOrEmpty connectionString then
            failwith "No valid connection string variable was supplied"

        new MySqlConnection(connectionString)

    let query sql parameters =
        use connection = Environment.GetEnvironmentVariable "CONNECTION_STRING" |> createConnection
        connection.Open()

        dapperMapParametrizedQuery connection sql parameters
