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

#include <queue>
#include <vector>
#include <iostream>
#include <stdio.h>
#include <functional>
#include <iterator> 

#include "Schedule.h"

using namespace std;

/*
	Name: Priority in scope of Priority
	Authors: Michael Beaver
	Purpose: Default Constructor. Initialize variables to default values.
		Scott did not write this function, but it had to be written for
		the system to be functional.
	Input: N/A
	Output: N/A
	Return: N/A
*/
Priority::Priority() {

	totalWait = 0;
	totalTurn = 0;
	totalBurst = 0;
	totalTime = 0;

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;

}



/*
	Name: Priority in scope of Priority
	Authors: Scott Smoke
	Purpose: Overloaded Constructor. Sets the schedule's ready queue and
		initializes other variables to default.
	Input: readyQueue is the ready queue from which to initialize.
	Output: readyQueue is the ready queue from which to initialize.
	Return: N/A
*/
Priority::Priority(const vector<PCB> & readyQueue) {

	readyVector = readyQueue; // list of processes
	numOfProcesses = readyVector.size();
	genRandomPriority();

	totalWait = 0;
	totalTurn = 0;
	totalBurst = 0;

}



/*
	Name: runSchedule in scope of Priority
	Authors: Scott Smoke
	Purpose: This simulates the preemptive priority schedulinng algorithm. This will create
		a priority queue by taking data from a vector and will sort the data as follows:
		lower numbered priorities have higher priority in the priority queue. 
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Priority::runSchedule() {

	unsigned int i = 0;		// index and time counter
	int waitTime = 0;		// wait time for each process (temporary)
	int turnTime = 0;		// turnaround time for each process (temporary)
	int pr = 0;				// compare variable

	priority_queue <PCB, vector<PCB>, less<vector<PCB>::value_type>> p;  //ready queue

	do {

		if (i < readyVector.size()) {

			pr = readyVector.at(i).getPriority();

			if (!p.empty() && pr > p.top().getPriority()) {

				readyVector.at(i).setArrTime(i);		// sets arrival time for which the item arrived in the queue
				p.top().setProState(-1);				// sets process to waiting
				p.push(readyVector.at(i));
				p.top().setProState(1);					// sets new head process to running
				
			}

			else {

				readyVector.at(i).setArrTime(i);
				p.push(readyVector.at(i));				// takes processes from the process pool into the readyQueue;
			
			}

		}

		p.top().setProState(1);		// process is running
		
		p.top().setExecutionTime(p.top().getExecution() - 1);	// simulates the process running
		
		if (p.top().getExecution() == 0) {

			waitTime = i - p.top().getBurstTime() - p.top().getArrTime();			// wait = finish time - cpu time - arrival time (cplusplus.com)
			turnTime = waitTime + p.top().getBurstTime() + p.top().getArrTime();	// turnaround = wait + cpu time + arrival
			totalTurn += turnTime;													// total for all processes
			totalWait += waitTime;													// total for all processes
			totalBurst += p.top().getBurstTime();
			p.pop();																// dequeues if the process has finished

		}

		i++;   // total time of processing also index variable
	
	} while(!p.empty());

	if (readyVector.size() -1 != 0) {      // protects against division by 0

		avgWait = float(totalWait) / float(numOfProcesses);
		avgTurn = float(totalTurn) / float(numOfProcesses);
		avgBurst = float(totalBurst) / float(numOfProcesses);
		avgResp = float(totalWait + i) / float(numOfProcesses);

	} 

	else {

		avgWait = 0;
		avgTurn = 0;
		avgBurst = 0;
		avgResp = 0;

	}

}



/*
	Name: genRandomPriority in scope of Priority
	Authors: Scott Smoke
	Purpose: Generates an arbitrary random priority for use by the
		priority scheduling algorithm.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Priority::genRandomPriority() {

	srand(rand());	//seed

	int r = rand();

	for (int i = 0; i < numOfProcesses; i++) {

		srand(r); //reseed
		r = rand();

		readyVector[i].setPriority(r % 100);

	}
	
}



/*
	Name: report in scope of Priority
	Authors: Scott Smoke
	Purpose: Prints out basic statistical information after running
		the priority scheduling algorithm.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void Priority::report() {

		cout << "Total wait Time\t\t" << totalWait <<endl;
		cout << "avg wait time:\t\t" << avgWait <<endl;
		cout << "Total Turnaround time\t" <<totalTurn<<endl;
		cout << "Avg turnaround Time\t" << avgTurn <<endl;
		cout << "Total burst time\t" << totalBurst <<endl;
		cout <<  "Avg burst time\t\t" << avgBurst <<endl;

		cout << "Total number of processes executed\t" << readyVector.size() <<endl;

}



/*
	Name: setReadyQueue in scope of Priority
	Authors: Michael Beaver
	Purpose: Initializes the schedule's ready queue from a given ready queue.
		Scott did not write this function, but it had to be written for
		the system to be functional.
	Input: newVector is the new ready queue.
	Output: newVector is the new ready queue.
	Return: N/A (void)
*/
void Priority::setReadyQueue(const std::vector<PCB> & newVector) {

	// Empty the queue first
	for (unsigned int i = 0; i < readyQueue.size(); i++)
		(this->readyQueue).pop();

	// Copy values to ready queue
	int qSize = newVector.size();
	for (int i = 0; i < qSize; i++)
		(this->readyVector).push_back(newVector[i]);

	numOfProcesses = readyVector.size();
	genRandomPriority();

}

