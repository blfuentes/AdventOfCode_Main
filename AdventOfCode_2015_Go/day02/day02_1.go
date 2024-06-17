package day02

import (
	"AdventOfCode_2015_Go/utilities"
	"regexp"
	"sort"
	"strconv"
)

type Present struct {
	dimensions []int64
	smallest   int64
}

func Executepart1() int {
	var fileName string = "./day02/day02.txt"
	// var fileName string = "./day02/test_input_02.txt"
	var result int = 0

	if fileContent, err := utilities.ReadFileAsLines(fileName); err == nil {
		for _, line := range fileContent {
			tmp := ExtractParts(line)
			result += CalculatePresent(tmp)
		}
	}

	return result
}

func ExtractParts(input string) Present {
	r, _ := regexp.Compile(`\d+`)
	dim := make([]int64, 3)
	areas := make([]int64, 3)

	if parts := r.FindAllString(input, 3); parts != nil {
		dim[0], _ = strconv.ParseInt(parts[0], 10, 0)
		dim[1], _ = strconv.ParseInt(parts[1], 10, 0)
		dim[2], _ = strconv.ParseInt(parts[2], 10, 0)

		areas[0] = dim[0] * dim[1]
		areas[1] = dim[1] * dim[2]
		areas[2] = dim[0] * dim[2]

		sort.Sort(utilities.Int64Array(areas))

		return Present{areas, areas[0]}
	}

	return Present{}
}

func CalculatePresent(present Present) int {
	return int(present.smallest) +
		2*int(present.dimensions[0]) +
		2*int(present.dimensions[1]) +
		2*int(present.dimensions[2])
}
