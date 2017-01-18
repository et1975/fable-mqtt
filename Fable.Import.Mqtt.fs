namespace Fable.Import
open System
open System.Text.RegularExpressions
open Fable.Core
open Fable.Import.JS

module mqtt =
    open Node.NodeJS

    type [<AllowNullLiteral>] Packet =
        abstract messageId: string with get, set
        [<Emit("$0[$1]{{=$2}}")>] abstract Item: key: string -> obj with get, set

    and [<AllowNullLiteral>] Granted =
        abstract topic: string with get, set
        abstract qos: float with get, set

    and [<AllowNullLiteral>] Topic =
        [<Emit("$0[$1]{{=$2}}")>] abstract Item: topic: string -> float with get, set

    and [<AllowNullLiteral>] ClientOptions =
        inherit SecureClientOptions
        abstract keepalive: float option with get, set
        abstract clientId: string option with get, set
        abstract protocolId: string option with get, set
        abstract protocolVersion: float option with get, set
        abstract clean: bool option with get, set
        abstract reconnectPeriod: float option with get, set
        abstract connectTimeout: float option with get, set
        abstract username: string option with get, set
        abstract password: string option with get, set
        abstract incomingStore: Store option with get, set
        abstract outgoingStore: Store option with get, set
        abstract will: obj option with get, set

    and [<AllowNullLiteral>] SecureClientOptions =
        abstract keyPath: string option with get, set
        abstract certPath: string option with get, set
        abstract rejectUnauthorized: bool option with get, set

    and [<AllowNullLiteral>] ClientPublishOptions =
        abstract qos: float option with get, set
        abstract retain: bool option with get, set

    and [<AllowNullLiteral>] ClientSubscribeOptions =
        abstract qos: float option with get, set

    and [<AllowNullLiteral>] ClientSubscribeCallback =
        [<Emit("$0($1...)")>] abstract Invoke: err: obj * granted: Granted -> unit

    and [<AllowNullLiteral>] Client =
        inherit EventEmitter
        [<Emit("$0($1...)")>] abstract Invoke: streamBuilder: obj * options: ClientOptions -> Client
        abstract publish: topic: string * message: Buffer * ?options: ClientPublishOptions * ?callback: Function -> Client
        abstract publish: topic: string * message: string * ?options: ClientPublishOptions * ?callback: Function -> Client
        abstract subscribe: topic: string * ?options: ClientSubscribeOptions * ?callback: ClientSubscribeCallback -> Client
        abstract subscribe: topic: ResizeArray<string> * ?options: ClientSubscribeOptions * ?callback: ClientSubscribeCallback -> Client
        abstract subscribe: topic: Topic * ?options: ClientSubscribeOptions * ?callback: ClientSubscribeCallback -> Client
        abstract unsubscribe: topic: string * ?options: ClientSubscribeOptions * ?callback: ClientSubscribeCallback -> Client
        abstract unsubscribe: topic: ResizeArray<string> * ?options: ClientSubscribeOptions * ?callback: ClientSubscribeCallback -> Client
        abstract ``end``: ?force: bool * ?callback: Function -> Client
        abstract handleMessage: packet: Packet * callback: Function -> Client

    and [<AllowNullLiteral>] Store =
        abstract put: packet: Packet * callback: Function -> Store
        abstract get: packet: Packet * callback: Function -> Store
        abstract createStream: unit -> ReadableStream
        abstract del: packet: Packet * callback: Function -> Store
        abstract close: callback: Function -> unit

    and [<AllowNullLiteral>] ConnectOptions =
        abstract protocolId: string option with get, set
        abstract protocolVersion: float option with get, set
        abstract keepalive: float option with get, set
        abstract clientId: string option with get, set
        abstract will: obj option with get, set
        abstract clean: bool option with get, set
        abstract username: string option with get, set
        abstract password: string option with get, set

    and [<AllowNullLiteral>] ConnectionPublishOptions =
        abstract messageId: float option with get, set
        abstract topic: string option with get, set
        abstract payload: string option with get, set
        abstract qos: float option with get, set
        abstract retain: bool option with get, set

    and [<AllowNullLiteral>] Connection =
        inherit EventEmitter
        abstract connect: ?options: ConnectOptions -> Connection
        abstract connack: ?options: obj -> Connection
        abstract publish: ?options: ConnectionPublishOptions -> Connection

    and [<AllowNullLiteral>] Server =
        inherit EventEmitter

    type [<Import("mqtt","mqtt")>] Globals =
//        abstract createClient: ?port: float, ?host: string, ?options: ClientOptions): Client = failwith "JS only"
//        abstract createSecureClient(?port: float, ?host: string, ?options: SecureClientOptions): Client = failwith "JS only"
        abstract connect: brokerUrl: string -> Client
        abstract connect: brokerUrl: string * opts: ClientOptions -> Client
//        abstract createConnection: port:float
//        abstract createConnection(?port: float, ?host: string, ?callback: Function): Connection = failwith "JS only"
//        abstract createServer(?listener: Function): Server = failwith "JS only"
//        abstract createSecureServer(keyPath: string, certPath: string, ?listener: Function): Server = failwith "JS only"


