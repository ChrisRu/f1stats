module Database
    open System
    open System.Dynamic
    open System.Collections.Generic
    open Dapper
    open MySql.Data

    let dapperQuery<'Result> (query : string) (connection : MySqlClient.MySqlConnection) =
        connection.QueryAsync<'Result>(query)

    let dapperParametrizedQuery<'Result> (query : string) (param : obj) (connection : MySqlClient.MySqlConnection) =
        connection.QueryAsync<'Result>(query, param)

    let dapperMapParametrizedQuery<'Result> (query : string) (param : Map<string, _>) (connection : MySqlClient.MySqlConnection) =
        let expando = ExpandoObject()
        let expandoDictionary = expando :> IDictionary<string, obj>
        for paramValue in param do
            expandoDictionary.Add(paramValue.Key, paramValue.Value :> obj)

        connection |> dapperParametrizedQuery query expando

    let createConnection connectionString =
        if String.IsNullOrEmpty(connectionString) then
            failwith "No valid connection string variable was supplied"

        let connection = new MySqlClient.MySqlConnection(connectionString)
        connection.Open()

        connection
