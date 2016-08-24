/*
	Names: Michael Beaver, Scott Smoke, and Wenhao Wang
	CS420, Fall 2013
	Assignment: CPU Scheduling Algorithms
	Description: This program runs simulations of the Shortest Job First, Priority, and Round Robin
		CPU scheduling algorithms. The user may either load a ready queue by file or have the 
		program randomly generate some ready queues. The number of random ready queues simulated is 
		user-specified, but the data in the Process Control Blocks (PCBs) is randomly generated. 
		The user has the option to compare a specified number of Round Robin time slices against 
		each other. A table of statistical data is output after the simulations have completed.

		Each line in the data file represents a process' PCB. There should be five (5) elements per line.
		PID    State    CPU Burst    Arrival Time    Priority

		Example:
		1 0 54 2 4 // PID of 1; State of 0; CPU Burst of 54; Arrival Time of 2; Priority of 4
    Due Date: December 3, 2013
*/

#include "Schedule.h"

/*
	Name: calcWaitTime in scope of Schedule
	Authors: Michael Beaver
	Purpose: Calculates the average wait time of a ready queue.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Schedule::calcWaitTime() {

	int wait = 0;
	int qSize = (this->readyVector).size();

	for (int i = 0; i < qSize; i++)
		wait += (this->readyVector[i]).getWaitTime();

	if (qSize > 0)
		this->avgWait = float(wait) / float(qSize);
	else
		this->avgWait = 0;
	
}



/*
	Name: calcRespTime in scope of Schedule
	Authors: Michael Beaver
	Purpose: Calculates the average response time of a ready queue.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Schedule::calcRespTime() {

	int response = 0;
	int qSize = (this->readyVector).size();

	for (int i = 0; i < qSize; i++)
		response += (this->readyVector[i]).getRespTime();

	if (qSize > 0)
		this->avgResp = float(response) / float(qSize);
	else
		this->avgResp = 0;

}



/*
	Name: calcTurnTime in scope of Schedule
	Authors: Michael Beaver
	Purpose: Calculates the average turnaround time of a ready queue.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Schedule::calcTurnTime() {

	int turnaround = 0;
	int qSize = (this->readyVector).size();

	for (int i = 0; i < qSize; i++)
		turnaround += (this->readyVector[i]).getTurnTime();

	if (qSize > 0)
		this->avgTurn = float(turnaround) / float(qSize);
	else
		this->avgTurn = 0;

}



/*
	Name: calcBurstTime in scope of Schedule
	Authors: Michael Beaver
	Purpose: Calculates the average CPU burst time of a ready queue.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Schedule::calcBurstTime() { 
	
	int sum = 0;
	int qSize = (this->readyVector).size();

	for (int i = 0; i < qSize; i++) 
		sum += (this->readyVector[i]).getBurstTime();

	if (qSize > 0)
		this->avgBurst = float(sum) / float(qSize);
	else
		this->avgBurst = 0;

}



/*
	Name: getAvgWait in scope of Schedule
	Authors: Michael Beaver
	Purpose: Returns the schedule's average wait time.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's average wait time.
*/
float Schedule::getAvgWait() const { 
	
	return this->avgWait; 

}


		
/*
	Name: getAvgResp in scope of Schedule
	Authors: Michael Beaver
	Purpose: Returns the schedule's average response time.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's average response time.
*/
float Schedule::getAvgResp() const { 
	
	return this->avgResp; 

}



/*
	Name: getAvgTurn in scope of Schedule
	Authors: Michael Beaver
	Purpose: Returns the schedule's average turnaround time.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's average turnaround time.
*/
float Schedule::getAvgTurn() const { 
	
	return this->avgTurn; 

}



/*
	Name: getAvgBurst in scope of Schedule
	Authors: Michael Beaver
	Purpose: Returns the schedule's average CPU burst time.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's average CPU burst time.
*/
float Schedule::getAvgBurst() const { 
	
	return this->avgBurst; 

}



/*
	Name: getNumProcs in scope of Schedule
	Authors: Michael Beaver
	Purpose: Returns the schedule's number of processes.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's number of processes.
*/
int Schedule::getNumProcs() const {

	return (this->readyVector).size();

}



/*
	Name: getTotalTime in scope of Schedule
	Authors: Michael Beaver
	Purpose: Returns the schedule's total execution time.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's total execution time.
*/
int Schedule::getTotalTime() const {

	return this->totalTime;

}


/*
	Name: setReadyQueue in scope of Schedule
	Authors: Michael Beaver
	Purpose: Initializes the schedule's ready queue from a given ready queue.
	Input: newVector is the new ready queue.
	Output: newVector is the new ready queue.
	Return: N/A (void)
*/
void Schedule::setReadyQueue(const std::vector<PCB> & newVector) {

	// Empty the queue first
	for (unsigned int i = 0; i < readyQueue.size(); i++)
		(this->readyQueue).pop();

	// Copy values to ready queue
	int qSize = newVector.size();
	for (int i = 0; i < qSize; i++)
		(this->readyQueue).push(newVector[i]);

}

