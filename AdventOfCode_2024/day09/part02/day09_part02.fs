module day09_part02

open AdventOfCode_2024.Modules

let parseContent(line: string) =
    let numFiles = (line.Length + 1) / 2
    let files = Array.zeroCreate numFiles
    let freefiles = Array.zeroCreate (numFiles - 1)
    for fIdx in 0..numFiles-1 do
        files[fIdx] <- (int)(line[2*fIdx].ToString())
        if fIdx < numFiles - 1 then
            freefiles[fIdx] <- (int)(line[2*fIdx+1].ToString())
    (files, freefiles)

let buildBlocksAndPositions (numOfBlocks: int) (numOfFiles: int) (files: int array) (freefiles: int array) =
    let blocks = Array.create numOfBlocks -1
    let positions = Array.zeroCreate numOfFiles
    let mutable block = 0

    for fIdx in 0 .. (numOfFiles - 1) do
        positions[fIdx] <- block
        for b in block .. block + files[fIdx] - 1 do
            blocks[b] <- fIdx
        block <- block + files[fIdx]
        if fIdx < numOfFiles - 1 then
            block <- block + freefiles[fIdx]
    (blocks, positions)    

let countFreeBlocks (blocks: int[]) from =
    let mutable freeblocks = 0
    let mutable isdone = false
    
    while not isdone do
        if from + 1 >= blocks.Length || blocks[from + freeblocks] > -1 then
            isdone <- true
        else
            freeblocks <- freeblocks + 1
    
    freeblocks

let gapOfSizeAvailable (blocks: int[]) size position =
    let mutable block = 0
    let mutable finished = false
    let mutable result : int option = None

    while not finished do
        if block >= position then
            finished <- true
        elif blocks[block] > -1 then
            block <- block + 1
        else
            let numFree = countFreeBlocks blocks block
            if numFree >= size then
                result <- Some block
                finished <- true
            else
                block <- block + numFree
    result

let execute() =
    let path = "day09/day09_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let numFiles = (content.Length + 1) / 2

    let (files, freefiles) = parseContent content
    
    let allfilengths = Array.sum files
    
    let blocks = allfilengths + Array.sum freefiles
    
    let (initBlocks, fPositions) = 
        buildBlocksAndPositions blocks numFiles files freefiles

    let blocks = Array.copy initBlocks
    for fIdx in (numFiles - 1) .. -1 .. 1 do
        let requiredGapSize = files[fIdx]
        let filePosition = fPositions[fIdx]
        match gapOfSizeAvailable blocks requiredGapSize filePosition with
        | None -> ()
        | Some firstFree ->
            for gapIdx in 0 .. requiredGapSize - 1 do
                blocks[firstFree + gapIdx] <- fIdx
                blocks[filePosition + gapIdx] <- -1

    blocks
    |> Array.mapi(fun idx v -> if v = -1 then 0I else (bigint(idx) * bigint.Parse(v.ToString())))
    |> Array.sum