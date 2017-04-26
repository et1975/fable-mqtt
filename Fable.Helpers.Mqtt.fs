module Fable.Helpers.Mqtt
    
open Fable.Core
open Fable.Import.JS
open Fable.Import.Mqtt
open Fable.Core.JsInterop

type ClientOptions =
    | Keepalive of float
    | ClientId of string
    | ProtocolId of string
    | ProtocolVersion of float
    | Clean of bool option
    | ReconnectPeriod of float
    | ConnectTimeout of float
    | Username of string
    | Password of string
    | IncomingStore of Store
    | OutgoingStore of Store
    | Will of obj
    interface IClientOptions

type SecureClientOptions =
    | KeyPath of string
    | CertPath of string
    | RejectUnauthorized of bool
    interface IClientOptions

type ClientPublishOptions =
    | Qos of float
    | Retain of bool
    interface IClientPublishOptions

type ClientSubscribeOptions =
    | Qos of float
    interface IClientSubscribeOptions


let [<PassGenerics>] callback (handler: string -> 'a -> unit) = 
    System.Func<string,string,unit>(fun topic msg -> ofJson msg |> handler topic)

[<Import("*","mqtt")>]
let mqtt : Static = jsNative

module Options =

    let inline ofList (opts:'t list) =
        keyValueList CaseRules.LowerFirst opts :?> 't

module Listener =
    let inline ofFunc f =
        System.Func<string,string,unit> f |> unbox
