module day10_part01

open AdventOfCode_2017.Modules

let rec processLengths (state: int array) (lengths: int list) (currentskip: int) (currentindex: int) =
    match lengths with
    | [] -> state
    | length :: rest ->
        let totake = length
        let (takeleft, takeright) = 
            if totake > (state.Length - currentindex) then 
                (totake - (totake - (state.Length - currentindex)), totake - (state.Length - currentindex) - 1)
            else
                (0, totake)
        let (lefttaken, righttaken) = (state |> Array.take(takeleft), state[currentindex..(currentindex + takeright - 1)])
        let mid = state[..currentindex - 1] |> Array.skip(takeleft)
        let ending = if takeleft = 0 then state[(currentindex+totake)..] else [||]
        let newstate = 
            if takeleft = 0 then
                Array.concat([lefttaken |> Array.rev; mid; righttaken |> Array.rev; ending])
            else
                Array.concat([righttaken |> Array.rev; mid; lefttaken |> Array.rev; ending])
        processLengths newstate rest (currentskip + 1) ((currentindex + length + currentskip) % state.Length)

let execute =
    let path = "day10/test_input_01.txt"
    //let path = "day10/day10_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split(", ") |> Array.map int |> List.ofArray
    let size = 5
    let circular = (Array.create size 0) |> Array.mapi(fun idx value -> idx)
    processLengths circular content 0 0