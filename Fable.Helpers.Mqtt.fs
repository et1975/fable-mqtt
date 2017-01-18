module internal Fable.Helpers.Mqtt
    
open Fable.Core
open Fable.Import.JS

[<Import("*","mqtt")>]
let mqtt : Fable.Import.mqtt.Globals = failwith "JS only"

let callback (handler: string -> 'a -> unit) = System.Func<string,string,unit>(fun topic msg -> JSON.parse msg :?> 'a |> handler topic)