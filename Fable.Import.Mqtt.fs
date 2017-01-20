namespace Fable.Import
open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.JS

module Mqtt =
    open Node.NodeJS

    type [<AllowNullLiteral>] Packet =
        abstract messageId: string with get, set
        [<Emit("$0[$1]{{=$2}}")>] abstract Item: key: string -> obj with get, set

    and [<AllowNullLiteral>] Granted =
        abstract topic: string with get, set
        abstract qos: float with get, set

    and [<AllowNullLiteral>] Topic =
        [<Emit("$0[$1]{{=$2}}")>] abstract Item: topic: string -> float with get, set

    and IClientOptions = interface end

    and [<KeyValueList>] ClientPublishOptions =
        | Qos of float
        | Retain of bool

    and [<KeyValueList>] ClientSubscribeOptions =
        | Qos of float

    and [<AllowNullLiteral>] ClientSubscribeCallback =
        [<Emit("$0($1...)")>] abstract Invoke: err: obj * granted: Granted -> unit

    and [<AllowNullLiteral>] Client =
        inherit EventEmitter
        [<Emit("$0($1...)")>] abstract Invoke: streamBuilder: obj * options: IClientOptions -> Client
        abstract publish: topic: string * message: Buffer * ?options: ClientPublishOptions * ?callback: Function -> Client
        abstract publish: topic: string * message: string * ?options: ClientPublishOptions * ?callback: Function -> Client
        abstract subscribe: topic: string * ?options: ClientSubscribeOptions list * ?callback: ClientSubscribeCallback -> Client
        abstract subscribe: topic: ResizeArray<string> * ?options: ClientSubscribeOptions list * ?callback: ClientSubscribeCallback -> Client
        abstract subscribe: topic: Topic * ?options: ClientSubscribeOptions list * ?callback: ClientSubscribeCallback -> Client
        abstract unsubscribe: topic: string * ?options: ClientSubscribeOptions list * ?callback: ClientSubscribeCallback -> Client
        abstract unsubscribe: topic: ResizeArray<string> * ?options: ClientSubscribeOptions list * ?callback: ClientSubscribeCallback -> Client
        abstract ``end``: ?force: bool * ?callback: Function -> Client
        abstract handleMessage: packet: Packet * callback: Function -> Client

    and [<AllowNullLiteral>] Store =
        abstract put: packet: Packet * callback: Function -> Store
        abstract get: packet: Packet * callback: Function -> Store
        abstract createStream: unit -> ReadableStream
        abstract del: packet: Packet * callback: Function -> Store
        abstract close: callback: Function -> unit

    and [<KeyValueList>] ConnectOptions =
        | ProtocolId of string
        | ProtocolVersion of float
        | Keepalive of float
        | ClientId of string
        | Will of obj
        | Clean of bool
        | Username of string
        | Password of string

    and [<KeyValueList>] ConnectionPublishOptions =
        | MessageId of float
        | Topic of string
        | Payload of string
        | Qos of float
        | Retain of bool

    and [<AllowNullLiteral>] Connection =
        inherit EventEmitter
        abstract connect: ?options: ConnectOptions list -> Connection
        abstract connack: ?options: obj -> Connection
        abstract publish: ?options: ConnectionPublishOptions list -> Connection

    and [<AllowNullLiteral>] Server =
        inherit EventEmitter

    type [<Import("mqtt","mqtt")>] Static =
        abstract connect: brokerUrl: string -> Client
        abstract connect: brokerUrl: string * opts: IClientOptions list -> Client