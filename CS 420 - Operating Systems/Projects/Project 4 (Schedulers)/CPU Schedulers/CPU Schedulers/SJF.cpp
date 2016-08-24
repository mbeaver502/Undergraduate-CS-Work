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

#include "Schedule.h"
#include "PCB.h"

using namespace std;

/*
	Name: SJF in scope of SJF
	Authors: Michael Beaver
	Purpose: Default Constructor. Initializes variables to default.
		Wenhao did not write this function, but it had to be written
		for the system to be functional.
	Input: N/A
	Output: N/A
	Return: N/A
*/
SJF::SJF() {

	avgWait = 0;
	avgResp = 0;
	avgTurn = 0;
	avgBurst = 0;
	totalTime = 0;

}



/*
	Name: SJF in scope of SJF
	Authors: Michael Beaver
	Purpose: Overloaded Constructor. Sets the ready queue and other
		variables to default. Wenhao did not write this function, 
		but it had to be written for the system to be functional.
	Input: newQueue is the new ready queue.
	Output: newQueue is the new ready queue.
	Return: N/A
*/
SJF::SJF(const std::vector<PCB> & newQueue) {

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
	Name: runSchedule in scope of SJF
	Authors: Wenhao Wang
	Purpose: Simulates the Shortest Job First CPU scheduling algorithm.
	Input: N/A
	Output: N/A
	Return: N/A (void)
*/
void SJF::runSchedule() {

	PCB proc;
	unsigned int qSize = readyQueue.size();

	// wait time & turn around time
	int * Bt = new int[qSize];
	int * Wt = new int[qSize];
    float k, t;					// k = the sum of wait time, t = the sum of burst time

	for (unsigned int i = 0; i < qSize; i++) {

		proc = readyQueue.front();
		Bt[i] = proc.getBurstTime();
        readyQueue.pop();

	}

	// sort burst time	
    for (unsigned int i = 0; i < qSize; i++) {

        for (unsigned int j = 0; j < qSize; j++) {

            if (Bt[i] < Bt[j]) {

                k = float(Bt[i]);
                Bt[i] = Bt[j];
                Bt[j] = int(k);

            }

        }

    }
    
    // calculate the wait time for each process
    t = 0; 
	k = 0; 
	Wt[0] = Bt[0];

    for (unsigned int i = 1; i < qSize; i++) {

        Wt[i] = Wt[i-1] + Bt[i];
		k += Wt[i];		// sum of wait time
		t += Bt[i];		// sum of turnaround time, t is sum of burst time

	}
    
	if (qSize > 0) {

		avgWait = float(k) / float(qSize);
		avgResp = float(k) / float(qSize);
		avgTurn = float(t + k) / float(qSize);

	}

	else {

		avgWait = 0;
		avgResp = 0;
		avgTurn = 0;

	}

	delete [] Bt;
	delete [] Wt;

}



/*
	Name: setReadyQueue in scope of SJF
	Authors: Michael Beaver
	Purpose: Initializes the schedule's ready queue from a given ready queue.
		Wenhao did not write this function, but it had to be written
		for the system to be functional.
	Input: newVector is the new ready queue.
	Output: newVector is the new ready queue.
	Return: N/A (void)
*//*
void SJF::setReadyQueue(const std::vector<PCB> & newVector) {

	// Empty the queue first
	for (unsigned int i = 0; i < readyQueue.size(); i++)
		(this->readyQueue).pop();

	// Copy values to ready queue
	int qSize = newVector.size();
	for (int i = 0; i < qSize; i++)
		(this->readyQueue).push(newVector[i]);

}
*/