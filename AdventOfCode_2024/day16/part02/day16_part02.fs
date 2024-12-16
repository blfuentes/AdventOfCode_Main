module day16_part02
open AdventOfCode_2024.Modules
open System.Collections.Generic

type CellType = 
    | WALL
    | EMPTY
    | START
    | END

type Cell = {
    Kind: CellType
    Row: int;
    Col: int;
    Orientation : int
}

let parseContent(lines: string array) =
    let maxrows = lines.Length
    let maxcols = lines[0].Length
    let map = Array2D.init maxrows maxcols (fun row col -> { Kind = WALL; Row = row; Col = col; Orientation = -1 })
    let mutable Start = { Kind = START; Row = -1; Col = -1; Orientation = 2 }
    let mutable End = { Kind = END; Row = -1; Col = -1; Orientation = 0 }
    for row in 0..(maxrows-1) do
        let line = lines[row].ToCharArray()        
        for col in 0..(maxcols-1) do
            let value = line[col]
            if value <> '#' then
                map[row, col] <- { map[row, col] with Kind = EMPTY } 
                if value = 'S' then
                    Start <- { Start with Row = row; Col = col }
                elif value = 'E' then
                    map[row, col] <- { map[row, col] with Kind = EMPTY } 
                    End <- { End with Row = row; Col = col; }
    (map, Start, End)

let minValueAtCell (dimensions: int[][][]) (endnode: Cell) =
    dimensions[endnode.Row][endnode.Col]
    |> Array.min
 
let walk ((startnode, endnode):Cell*Cell) (map: Cell[,]) =
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))

    let visited = Array.init maxrows (fun _ -> 
        Array.create maxcols System.Int32.MaxValue
    )
    
    let distances = Array.init maxrows (fun _ -> 
        Array.init maxcols (fun _ -> 
            Array.create 4 System.Int32.MaxValue
        )
    )
    let passed = Array.init maxrows (fun _ -> 
        Array.init maxcols (fun _ -> 
            Array.create 4 System.Int32.MaxValue
        )
    )
    
    let queueQ = new Queue<Cell>()
    let rowDirs = [|-1; 0; 1; 0 |]
    let colDirs = [|0; -1; 0; 1 |]

    let add (cell: Cell) (cost: int) (before: int)=
        let bitMask = 1 <<< before
        let currentCost = distances[cell.Row][cell.Col][cell.Orientation]
        //printfn "Row %d Col %d Orientation %d Current cost %d (is wall?) %b" cell.Row cell.Col cell.Orientation currentCost cell.Kind.IsWALL       
        if not cell.Kind.IsWALL then
            if currentCost > cost then
                distances[cell.Row][cell.Col][cell.Orientation] <- cost
                passed[cell.Row][cell.Col][cell.Orientation] <- 0
                //printfn "Row %d Col %d Orientation %d New cost %d" cell.Row cell.Col cell.Orientation cost
                queueQ.Enqueue(cell)
            if distances[cell.Row][cell.Col][cell.Orientation] = cost then
                passed[cell.Row][cell.Col][cell.Orientation] <- passed[cell.Row][cell.Col][cell.Orientation] ||| before

    let addVisited(cell: Cell) =
        let bitMask = 1 <<< cell.Orientation
        if (visited[cell.Row][cell.Col] &&& bitMask) <> 0 then
            () // ALREADY THERE!
        else
            visited[cell.Row][cell.Col] <- visited[cell.Row][cell.Col] ||| bitMask // VISITED!
            queueQ.Enqueue(cell)

    add startnode 0 4
    while queueQ.Count > 0 do
        let newcell = queueQ.Dequeue()
        let distance = distances[newcell.Row][newcell.Col][newcell.Orientation]
        let (newRow, newCol) = (newcell.Row + rowDirs[newcell.Orientation], newcell.Col + colDirs[newcell.Orientation])
        let cell1 = { 
            Kind = map[newRow, newCol].Kind        
            Row = newRow;
            Col = newCol;
            Orientation = newcell.Orientation
        }
        add cell1 (distance+1) newcell.Orientation

        let cell2 = { 
            Kind = newcell.Kind        
            Row = newcell.Row;
            Col = newcell.Col;
            Orientation = (newcell.Orientation + 1) % 4
        }
        add cell2 (distance+1000) newcell.Orientation

        let cell3 = { 
            Kind = newcell.Kind       
            Row = newcell.Row;
            Col = newcell.Col;
            Orientation = (newcell.Orientation + 3) % 4
        }
        add cell3 (distance+1000) newcell.Orientation
    
    // end walking, find best
    let minimal = (minValueAtCell distances endnode) - 1000
    [0..3]
    |> List.iter(fun d ->
        if (distances[endnode.Row][endnode.Col][d] = minimal) then
            addVisited { Row = endnode.Row; Col = endnode.Col; Orientation = d; Kind = EMPTY }
    )

    while queueQ.Count > 0 do
        let newcell = queueQ.Dequeue()
        [0..3]
        |> List.iter(fun d ->
            let bitMask = 1 <<< d
            if passed[newcell.Row][newcell.Col][newcell.Orientation] &&& bitMask <> 0 then
                let (newRow, newCol) = (newcell.Row - rowDirs[d], newcell.Col - colDirs[d])
                addVisited { Row = newRow; Col = newCol; Orientation = d; Kind = EMPTY }
        )
    (visited 
    |> Array.map (fun line -> line |> Array.filter (fun a -> a > 0) |> Array.length)
    |> Array.sum) - 1
    
    

let execute() =
    //let path = "day16/day16_input.txt"
    let path = "day16/test_input_16.txt"
    //let path = "day16/test_input_16b.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, startNode, endNode) = parseContent content

    walk (startNode, endNode) map