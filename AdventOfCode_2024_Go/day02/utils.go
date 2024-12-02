package day02

func IsSafeUpReport(report []int) bool {
	isSafe := true
	for idx := 0; idx < len(report)-1; idx++ {
		if 1 > (report[idx+1]-report[idx]) || (report[idx+1]-report[idx]) > 3 {
			isSafe = false
			break
		}
	}
	return isSafe
}

func IsSafeDownReport(report []int) bool {
	isSafe := true
	for idx := 0; idx < len(report)-1; idx++ {
		if 1 > (report[idx]-report[idx+1]) || (report[idx]-report[idx+1]) > 3 {
			isSafe = false
			break
		}
	}
	return isSafe
}

func IsSafeReport(report []int) bool {
	return IsSafeUpReport(report) || IsSafeDownReport(report)
}

func GetExcludedReport(index int, report []int) []int {
	if index < 0 || index >= len(report) {
		return []int{}
	}

	temp := make([]int, 0)
	for i, v := range report {
		if i != index {
			temp = append(temp, v)
		}
	}

	return temp
}

func IsSafeReportWithoutOne(report []int) bool {
	isSafe := false
	for idx := 0; idx < len(report); idx++ {
		if IsSafeReport(GetExcludedReport(idx, report)) {
			isSafe = true
			break
		}
	}
	return isSafe
}
