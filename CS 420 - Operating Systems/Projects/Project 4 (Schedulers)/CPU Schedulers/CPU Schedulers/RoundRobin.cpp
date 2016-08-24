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

#include <vector>
#include <queue>
#include <iostream>

#include "PCB.h"
#include "Schedule.h"

using namespace std;

//------------------------------------------------------------------------------------------------------

const unsigned int TIME_SLICE_MINIMUM = 10;

//------------------------------------------------------------------------------------------------------

/*
	Name: RoundRobin in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Default Constructor. Initializes variables to default values.
	Input: N/A
	Output: N/A
	Return: N/A
*/
RoundRobin::RoundRobin() {

	timeSlice = TIME_SLICE_MINIMUM;

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

}



/*
	Name: RoundRobin in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Overloaded Constructor. Initializes this schedule's ready
		queue to a given ready queue and other variables to default values.
	Input: newQueue is the new ready queue.
	Output: newQueue is the new ready queue.
	Return: N/A
*/
RoundRobin::RoundRobin(const std::queue<PCB> & newQueue) {

	timeSlice = TIME_SLICE_MINIMUM;

	// Copy values to ready queue
	queue<PCB> tempQ = newQueue;
	for (unsigned int i = 0; i < tempQ.size(); i++) {

		readyQueue.push(tempQ.front());
		tempQ.pop();

	}

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

}



/*
	Name: RoundRobin in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Overloaded Constructor. Initializes this schedule's ready
		queue to a given ready queue and other variables to default values.
	Input: newQueue is the new ready queue.
	Output: newQueue is the new ready queue.
	Return: N/A
*/
RoundRobin::RoundRobin(const std::vector<PCB> & newQueue) {

	timeSlice = TIME_SLICE_MINIMUM;

	// Copy values to ready queue
	unsigned int qSize = newQueue.size();
	for (unsigned int i = 0; i < qSize; i++) 
		readyQueue.push(newQueue[i]);

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

}



/*
	Name: RoundRobin in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Overloaded Constructor. Initializes this schedule's ready
		queue to a given ready queue, time slice to a given time,
		and other variables to default.
	Input: newQueue is the new ready queue, and time is the new time slice.
	Output: newQueue is the new ready queue.
	Return: N/A
*/
RoundRobin::RoundRobin(const std::queue<PCB> & newQueue, unsigned int time) {

	// Maintain a minimum time slice
	if (time < TIME_SLICE_MINIMUM)
		timeSlice = TIME_SLICE_MINIMUM;
	else
		timeSlice = time;

	// Copy values to ready queue
	queue<PCB> tempQ = newQueue;
	for (unsigned int i = 0; i < tempQ.size(); i++) {

		readyQueue.push(tempQ.front());
		tempQ.pop();

	}

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

}



/*
	Name: RoundRobin in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Overloaded Constructor. Initializes this schedule's ready
		queue to a given ready queue, time slice to a given time,
		and other variables to default.
	Input: newQueue is the new ready queue, and time is the new time slice.
	Output: newQueue is the new ready queue.
	Return: N/A
*/
RoundRobin::RoundRobin(const std::vector<PCB> & newQueue, unsigned int time) {

	// Maintain a minimum time slice
	if (time < TIME_SLICE_MINIMUM)
		timeSlice = TIME_SLICE_MINIMUM;
	else
		timeSlice = time;

	// Copy values to ready queue
	unsigned int qSize = newQueue.size();
	for (unsigned int i = 0; i < qSize; i++) 
		readyQueue.push(newQueue[i]);

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

}



/*
	Name: runSchedule in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Simulates the Round Robin CPU scheduling algorithm.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void RoundRobin::runSchedule() {

	PCB currentProc;
	int currentProcBurst = 0;
	int burstRemaining = 0;

	int timeElapsed = 0;
	int procCounter = 0;

	// Go through the ready queue
	while (readyQueue.size() > 0) {

		currentProc = readyQueue.front();
		currentProcBurst = currentProc.getRemBurst();

		// If a process is not ready, skip it
		if (currentProc.getProState() != 0) 
			continue;

		// Process is now running
		currentProc.setProState(1);

		currentProc.setWaitTime(timeElapsed);
		currentProc.setRespTime(currentProc.getArrTime() + currentProc.getWaitTime());

		burstRemaining = currentProcBurst - timeSlice;
		currentProc.setRemBurst(burstRemaining);

		// The process needs the CPU for time less than the time slice
		if (currentProcBurst <= timeSlice) {

			currentProc.setTurnTime(currentProc.getWaitTime() + currentProcBurst + currentProc.getArrTime());
			currentProc.setProState(0);

			// Current process is finished, remove it from queue
			readyVector.push_back(currentProc);
			readyQueue.pop();

			timeElapsed += currentProcBurst + currentProc.getArrTime();

		}

		// The process needs the CPU for time greater than the time slice
		else {

			currentProc.setTurnTime(currentProc.getWaitTime() + currentProc.getArrTime() + timeSlice);
			currentProc.setProState(0);

			// Current process is not finished, send it to the back of the queue
			readyQueue.pop();
			readyQueue.push(currentProc);

			timeElapsed += timeSlice + currentProc.getArrTime();

		}

	}
	
	// Calculate all averages
	this->calcWaitTime();
	this->calcRespTime();
	this->calcTurnTime();
	this->calcBurstTime();

	this->totalTime = timeElapsed;

}



/*
	Name: setReadyQueue in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Initializes the schedule's ready queue from a given ready queue.
	Input: newQueue is the new ready queue.
	Output: newQueue is the new ready queue.
	Return: N/A (void)
*/
void RoundRobin::setReadyQueue(const std::queue<PCB> & newQueue) {

	// Empty the queue first
	for (unsigned int i = 0; i < readyQueue.size(); i++)
		(this->readyQueue).pop();

	// Copy values to ready queue
	queue<PCB> tempQ = newQueue;
	for (unsigned int i = 0; i < tempQ.size(); i++) {

		(this->readyQueue).push(tempQ.front());
		tempQ.pop();

	}

}



/*
	Name: setReadyQueue in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Initializes the schedule's ready queue from a given ready queue.
	Input: newVector is the new ready queue.
	Output: newVector is the new ready queue.
	Return: N/A (void)
*/
void RoundRobin::setReadyQueue(const std::vector<PCB> & newVector) {

	// Empty the queue first
	for (unsigned int i = 0; i < readyQueue.size(); i++)
		(this->readyQueue).pop();

	// Copy values to ready queue
	int qSize = newVector.size();
	for (int i = 0; i < qSize; i++)
		(this->readyQueue).push(newVector[i]);

}



/*
	Name: setTimeSlice in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Sets the schedule's time slice.
	Input: time is the new time slice.
	Output: N/A
	Return: N/A (void)
*/
void RoundRobin::setTimeSlice(unsigned int time) {

	// Maintain a minimum time slice
	if (time < TIME_SLICE_MINIMUM)
		timeSlice = TIME_SLICE_MINIMUM;
	else
		timeSlice = time;

}



/*
	Name: resetData in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Resets data within the schedule.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void RoundRobin::resetData() {

	// Empty the queues
	for (unsigned int i = 0; i < readyQueue.size(); i++)
		readyQueue.pop();
	readyVector.clear();

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

	timeSlice = TIME_SLICE_MINIMUM;

}



/*
	Name: getTimeSlice in scope of RoundRobin
	Authors: Michael Beaver
	Purpose: Returns the schedule's time slice.
	Input: N/A
	Output: N/A
	Return: Returns the schedule's time slice.
*/
unsigned int RoundRobin::getTimeSlice() const {

	return timeSlice;

}

