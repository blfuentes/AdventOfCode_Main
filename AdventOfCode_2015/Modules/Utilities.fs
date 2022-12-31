module Utilities

open System.IO

let GetLinesFromFile(path: string) =
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path)

let GetContentFromFile(path: string) =
    File.ReadAllText(__SOURCE_DIRECTORY__ + @"../../" + path)

let everyNth (n: int) (l: 'a list) = 
  l |> List.mapi (fun i el -> el, i)                // Add index to element
    |> List.filter (fun (el, i) -> i % n = n - 1)   // Take every nth element
    |> List.map fst                                 // Drop index from the result

let splitEvenOddList (l: 'a list) = 
  let listwithIdx =
    l |> List.mapi (fun i el -> el, i)                // Add index to element
  ((listwithIdx |> List.filter (fun (el, i) -> i % 2 = 0)) |> List.map fst, (listwithIdx |> List.filter (fun (el, i) -> i % 2 <> 0)) |> List.map fst)


let rec comb n l =
  match (n,l) with
  | (0,_) -> [[]]
  | (_,[]) -> []
  | (n,x::xs) ->
      let useX = List.map (fun l -> x::l) (comb (n-1) xs)
      let noX = comb n xs
      (useX @ noX)

let areConsecutive (i: List<int>) =
    i |> List.pairwise |> List.forall(fun a -> (snd a) - (fst a) = 1)      