module day10_part01

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
        

let rec processLengths (state: Node array) (lengths: int list) (currentskip: int) (currentindex: int) =
    match lengths with
    | [] -> state
    | totake :: rest ->
        let indexes = 
            seq {
                for idx in [currentindex..currentindex + totake - 1] do
                    yield (state[idx % state.Length], idx % state.Length)
            } |> Array.ofSeq

        Array.iteri2(fun idx el rev -> state[snd el] <- fst rev) indexes (indexes |> Array.rev)

        processLengths state rest (currentskip + 1) ((currentindex + totake + currentskip) % state.Length)

let execute =
    let path = "day10/day10_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(",") |> Array.map(fun v ->  int v) |> List.ofArray
    let size = 256
    let circular = 
        (Array.create size { Index = 0; Value = 0; NextIndex = 0 }) 
        |> Array.mapi(fun idx value -> { Index = idx; Value = idx; NextIndex = (idx + 1) % size })
    processLengths circular content 0 0 |> ignore
    circular |> Array.take(2) |> Array.map _.Value |> Array.reduce (*)