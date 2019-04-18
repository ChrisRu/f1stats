module api.Settings

open System
open Microsoft.AspNetCore.Http

type QuerySettings = {
    limit : string;
    offset : string;
}

let inline (=>) k v = k, box v

let getSettings (query : IQueryCollection) =
    query
    |> Seq.map(
        fun f ->
            match query.TryGetValue f.Key with
                | true, value ->
                    let couldParse, value = Int32.TryParse(value.ToString())

                    let ignore _ = if couldParse then "ok" else invalidArg f.Key "Value supplied to offset is unknown"

                    value
                | _ -> 20
    )
    |> dict