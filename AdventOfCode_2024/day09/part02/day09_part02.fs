module day09_part02

open AdventOfCode_2024.Modules

let parseContent(line: string) =
    line.ToCharArray() |> Array.map (fun v -> System.Int32.Parse((string)v))

let buildblocks (input: int array) =
    let builder = new System.Text.StringBuilder()
    let mutable currentVIdx = 0

    input
    |> Array.mapi(fun idx v ->
        let replica =
            if idx = 0 then
                "0"
            else
                if idx % 2 = 0 then 
                    currentVIdx.ToString()                     
                else "."
        if replica <> "." then currentVIdx <- currentVIdx + 1
        Array.create v replica
    )

let splitParts (inputArray: string array array) =
    inputArray
    |> Array.collect(fun subarray ->
        subarray |> Array.groupBy id |> Array.map snd
    )

let processblocks (input: string array array) =
    let mutable touse = splitParts input
    let mutable blockIdx = touse.Length - 1
    let mutable gapIdx = 0
    while blockIdx > gapIdx do
        if blockIdx % 500 = 0 then
            printfn "block %d of %d" blockIdx input.Length
        while blockIdx > 0  && touse[blockIdx][0] = "." do
            blockIdx <- blockIdx - 1
        let blocksize = touse[blockIdx].Length
        while gapIdx < blockIdx && (not ((touse[gapIdx][0]).Equals(("."))) || touse[gapIdx].Length < blocksize) do
            gapIdx <- gapIdx + 1

        if blockIdx > 0 && blockIdx > gapIdx then
            let found = touse[blockIdx]
            for sIdx in [0..found.Length-1] do
                touse[gapIdx][sIdx] <- found[sIdx]
                touse[blockIdx][sIdx] <- "."
            touse <- splitParts touse
            gapIdx <- 0
        if gapIdx = blockIdx then
            blockIdx <- blockIdx - 1
            gapIdx <- 0
    touse |> Array.concat

let execute() =
    let path = "day09/day09_input.txt"
    //let path = "day09/test_input_01.txt"
    let content = LocalHelper.GetContentFromFile path
    let block = buildblocks (parseContent content)
    let processed = processblocks block
    //printfn "%s" (String.concat "" processed)
    processed
    |> Array.mapi(fun idx v -> if v = "." then 0I else (bigint(idx) * bigint.Parse(v)))
    |> Array.sum