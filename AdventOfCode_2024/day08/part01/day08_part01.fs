module day08_part01

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
    Display: char [,];
}

let parseContent (lines: string array) =
    let maxRows = lines.Length
    let maxCols = lines[0].Length

    let mapAntennas = Array2D.create maxRows maxCols '.'
    let antennas = 
        [for row in [0..maxRows-1] do
            for col in [0..maxCols-1] do
                let value = lines[row][col]
                mapAntennas[row, col] <- value
                if value <> '.' then
                    yield { Name = value; Position = {Row = row; Col = col} }]
    (mapAntennas, antennas)

let inBoundaries (coord: Coord) (maxRows: int) (maxCols: int) =
    coord.Row >= 0 && coord.Row < maxRows &&
    coord.Col >= 0 && coord.Col < maxCols

let mirror (coordA: Coord) (coordB: Coord) =
    let mirrored1 = (2 * coordB.Row - coordA.Row, 2 * coordB.Col - coordA.Col)
    let mirrored2 = (2 * coordA.Row - coordB.Row, 2 * coordA.Col - coordB.Col)

    (
        { Row = fst mirrored1; Col = snd mirrored1 },
        { Row = fst mirrored2; Col = snd mirrored2 }
    )

let printAntennaMap (map: char[,]) =
    for r in 0..map.GetLength(0)-1 do
        for c in 0..map.GetLength(1)-1  do
            printf "%c" map[r, c]
        printfn ""

let calculateAntinode (antennas: Antenna list) (antennaMap: char[,]) =
    let combinations = Utilities.combination 2 antennas
    let maxRows = antennaMap.GetLength(0)
    let maxCols = antennaMap.GetLength(1)

    let rAntennas =
        [for comb in combinations do
            let pair = comb |> Array.ofList
            let (fromFirst, fromSecond) = mirror pair[0].Position pair[1].Position
            if inBoundaries fromFirst maxRows maxCols then
                let value = antennaMap[fromFirst.Row, fromFirst.Col]
                if value = '.' then
                    antennaMap[fromFirst.Row, fromFirst.Col] <- '#'
                else
                    antennaMap[fromFirst.Row, fromFirst.Col] <- '@'
                if antennaMap[fromFirst.Row, fromFirst.Col] = '#' || antennaMap[fromFirst.Row, fromFirst.Col] = '@' then
                    yield fromFirst
            if inBoundaries fromSecond maxRows maxCols then
                let value = antennaMap[fromSecond.Row, fromSecond.Col]
                if value = '.' then
                    antennaMap[fromSecond.Row, fromSecond.Col] <- '#'
                else
                    antennaMap[fromSecond.Row, fromSecond.Col] <- '@'
                if antennaMap[fromSecond.Row, fromSecond.Col] = '#' || antennaMap[fromSecond.Row, fromSecond.Col] = '@' then
                    yield fromSecond
    ]
    rAntennas
    

let execute() =
    let path = "day08/day08_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (mapAntennas, antennas) = parseContent content
    let groups = antennas |> List.groupBy _.Name
    groups
    |> List.map(fun g ->
        calculateAntinode (snd g) mapAntennas
    )
    |> List.concat
    |> List.distinct
    |> List.length