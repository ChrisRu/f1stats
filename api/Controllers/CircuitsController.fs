module api.Controllers.CircuitsController

open api
open api.Database

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

let request options =
    let rawQuery = "SELECT * FROM circuits LIMIT @limit OFFSET @offset;"
    let param = Settings.getSettings options
    query<Circuit> rawQuery param

