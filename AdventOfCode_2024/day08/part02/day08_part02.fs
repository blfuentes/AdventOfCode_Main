module day08_part02

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

type Coord = {
    Row: int;
    Col: int;
}

type Antenna = {
    Name: char;
    Position : Coord;
}

type AntennaMap = {
    MaxRow: int;
    MaxCol: int;
}
let parseContent (lines: string array) =
    let maxRows = lines.Length
    let maxCols = lines[0].Length
    let antennas =
        [for row in [0..maxRows-1] do
            for col in [0..maxCols-1] do
                let value = lines[row][col]
                if value <> '.' then
                    yield { Name = value; Position = {Row = row; Col = col} }]
    ({ MaxRow = maxRows; MaxCol = maxCols }, antennas)

let inBoundaries (coord: Coord) (maxRows: int) (maxCols: int) =
    coord.Row >= 0 && coord.Row < maxRows &&
    coord.Col >= 0 && coord.Col < maxCols

let mirroredForRep (coordA: Coord) (coordB: Coord) repetition =
    let rowDistance = coordB.Row - coordA.Row
    let colDistance = coordB.Col - coordA.Col

    let mirrored1 = { Row = coordB.Row + repetition * rowDistance; Col = coordB.Col + repetition * colDistance }
    let mirrored2 = { Row = coordA.Row - repetition * rowDistance; Col = coordA.Col - repetition * colDistance }
    (mirrored1, mirrored2)

let calculateAntinode (antennas: Antenna list) (mapAntenna: AntennaMap) =
    let combinations = Utilities.combination 2 antennas
    let maxRows = mapAntenna.MaxRow
    let maxCols = mapAntenna.MaxCol

    let rAntennas =
        [for comb in combinations do
            let pair = comb |> Array.ofList
            let mutable fOutOfRange = false
            let mutable sOutOfRange = false
            let mutable expand = 1
            yield pair[0].Position
            yield pair[1].Position
            while not fOutOfRange || not sOutOfRange do                
                let (fromFirst, fromSecond) = mirroredForRep pair[0].Position pair[1].Position expand
                if not fOutOfRange && inBoundaries fromFirst maxRows maxCols then
                    yield fromFirst
                else
                    fOutOfRange <- true
                if not sOutOfRange && inBoundaries fromSecond maxRows maxCols then
                    yield fromSecond
                else
                    sOutOfRange <- true
                expand <- expand + 1
    ]
    rAntennas
    

let execute() =
    let path = "day08/day08_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (mapAntenna, antennas) = parseContent content
    let groups = antennas |> List.groupBy _.Name
    groups
    |> List.map(fun g ->
        calculateAntinode (snd g) mapAntenna
    )
    |> List.concat
    |> List.distinct
    |> List.length