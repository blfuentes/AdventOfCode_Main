module day10_part02

open AdventOfCode_2017.Modules

type Node = {
    Index: int;
    Value: int;
    NextIndex: int;
}

let getPart (state: Node array) (start: int) (count: int) =
    let part = 
        seq {
            for idx in [start..start+count] do
                yield state |> Array.find(fun s -> s.Index = idx % state.Length)
        }
    part |> Array.ofSeq
        

let rec processLengths (roundsleft: int) (state: Node array) (initiallengths: int list) (lengths: int list) (currentskip: int) (currentindex: int) =
    match lengths with
    | [] -> 
        if roundsleft > 0 then
            processLengths (roundsleft - 1) state initiallengths initiallengths currentskip currentindex
        else
            state
    | totake :: rest ->
        let indexes = 
            seq {
                for idx in [currentindex..currentindex + totake - 1] do
                    yield (state[idx % state.Length], idx % state.Length)
            } |> Array.ofSeq

        Array.iteri2(fun idx el rev -> state[snd el] <- fst rev) indexes (indexes |> Array.rev)

        processLengths roundsleft state  initiallengths rest (currentskip + 1) ((currentindex + totake + currentskip) % state.Length)

let execute =
    let path = "day10/day10_input.txt"
    let text = (LocalHelper.GetContentFromFile path)
    let extra = "17, 31, 73, 47, 23".Split(", ") |> Array.map(fun v ->  int v) |> List.ofArray
    let content = List.concat([text.ToCharArray() |> Array.map int |> List.ofArray;extra])
    let size = 256
    let rounds = 64
    let circular = 
        (Array.create size { Index = 0; Value = 0; NextIndex = 0 }) 
        |> Array.mapi(fun idx value -> { Index = idx; Value = idx; NextIndex = (idx + 1) % size })
    let cloned = content |> Array.ofList |> Array.copy |> List.ofArray
    processLengths (rounds - 1)  circular cloned content 0 0 |> ignore
    circular |> Array.take(2) |> Array.map _.Value |> Array.reduce (*) |> ignore
    let chunks = circular |> Array.map _.Value |> Array.chunkBySize(16)
    let reduced = chunks |> Array.map(fun c -> c |> Array.reduce (^^^))
    let hexadecimals = reduced |> Array.map(fun v -> sprintf "%02X" v) |> Seq.ofArray
    (String.concat "" hexadecimals).ToLowerInvariant()
