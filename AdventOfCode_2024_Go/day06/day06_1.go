package day06

import (
	"fmt"

	"github.com/blfuentes/AdventOfCode_2024_Go/utilities"
)

func Executepart1() int {
	var result int = 0

	var fileName string = "./day06/day06.txt"
	if fileContent, err := utilities.ReadFileAsText(fileName); err == nil {
		fmt.Println(len(fileContent))
	}

	return result
}
