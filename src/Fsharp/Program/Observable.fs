﻿namespace Fuse

open Fable.Core
open Fable.Import.JS    

module Observable =   

    type IObservable<'T> =        
        abstract member value : 'T with get, set        
        abstract member map<'T,'U> : ('T -> 'U) -> IObservable<'U>
        abstract member where: ('T -> bool) -> IObservable<'T>
        [<Emit("$0.map($1...)")>] abstract member mapi<'T, 'U> : ('T * int -> 'U) -> IObservable<'U> // This is NOT implemented correctly
        abstract member count: unit -> IObservable<int>
        [<Emit("$0.count($1...)")>] abstract member countWhere: ('T -> bool) -> IObservable<int>     
        abstract member add: 'T -> unit
        abstract member remove: 'T -> unit
        abstract member tryRemove: 'T -> bool
        abstract member removeAt: int -> unit
        abstract member removeWhere: ('T -> bool) -> unit
        abstract member refreshAll: IObservable<'T> -> ('T -> 'T -> bool) -> ('T -> 'T -> unit) -> ('T -> 'T) -> unit
        abstract member forEach : ('T -> unit) -> unit        
        abstract member clear : unit -> unit
        abstract member indexOf : 'T -> int
        abstract member contains : 'T -> bool
        abstract member filter : ('T -> bool) -> IObservable<'T>
        abstract member expand : unit -> IObservable<'T>
        abstract member addSubscriber: ('T -> unit) -> unit
        abstract member removeSubscriber: ('T -> unit) -> unit
        abstract member length: unit -> int
        abstract member toArray: unit -> 'T array
        abstract member getAt : int -> 'T
        abstract member replaceAt : int -> 'T -> unit
        abstract member replaceAll : 'T array -> unit

    type IUnsafeObservable<'T> = 
        inherit IObservable<'T>
        [<Emit("$0.value = $1")>] abstract member valueOverride : obj with get, set

    type IObservable = 
        inherit IObservable<obj>                

    type private ObservableFactory =
        abstract Invoke<'T> : element: 'T -> IObservable<'T>
        abstract Invoke<'T> : unit -> IObservable<'T>
        abstract InvokeUnsafe<'T> : element: 'T -> IUnsafeObservable<'T>
        abstract InvokeUnsafe<'T> : unit -> IUnsafeObservable<'T>
        abstract Invoke : unit -> IObservable
        [<Emit("(0, _Observable2.default)(...$1)")>] // This doesn't work
        abstract InvokeList<'T> : elements : 'T array -> IObservable<'T>

    type private Globals =
        static member observable with get(): ObservableFactory = failwith "JS only" and set(v: ObservableFactory): unit = failwith "JS only"

    [<Import("", "FuseJS/Observable")>] 
    let createWith<'T> (elem : 'T) = 
        Globals.observable.Invoke(elem)
 
    [<Import("", "FuseJS/Observable")>]
    let createTyped<'T> =
        Globals.observable.Invoke<'T>()

    [<Import("", "FuseJS/Observable")>] 
    let createUnsafeWith<'T> (elem : 'T) = 
        Globals.observable.InvokeUnsafe(elem)
 
    [<Import("", "FuseJS/Observable")>]
    let createUnsafeTyped<'T> =
        Globals.observable.InvokeUnsafe<'T>()

    [<Import("", "FuseJS/Observable")>] 
    let create () = 
        Globals.observable.Invoke()

    // This doesn't work
    [<Import("", "FuseJS/Observable")>] 
    let createList<'T> (elements : 'T array) =
        Globals.observable.InvokeList<'T>(elements)
    
