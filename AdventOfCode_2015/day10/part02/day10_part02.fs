module day10_part02

let read (input : string) =
    input
    |> Seq.fold (fun acc x ->
        match acc with
        | (n, x')::tl when x = x' -> (n+1, x')::tl
        | _ -> (1, x)::acc) []
    |> List.rev
    |> Seq.collect (fun (n, x) -> sprintf "%d%c" n x)
    |> fun xs -> System.String.Join("", xs)

let execute =
    {1..50}
    |> Seq.fold (fun acc _ -> read acc) "1321131112"
    |> Seq.length