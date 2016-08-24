/*
	Names: Michael Beaver, Scott Smoke, and Wenhao Wang
	CS420, Fall 2013
	Assignment: 
	Description: 
    Due Date: December 3, 2013
*/

#include <iostream>
#include <stdio.h>
#include <ctime>
#include <vector>
#include "PCB.h"
#include "Schedule.h"
#include <string>

using namespace std;

//------------------------------------------------------------------------------------------------------

const unsigned int MAX_PROCS = 500;
const unsigned int PID_BOUND = 1000;
const unsigned int BURST_BOUND = 100;

const unsigned int MAX_QUEUES = 9;
const unsigned int MIN_QUEUES = 1;

const string LINE = "-------------------------------------------------------------------------------";

//------------------------------------------------------------------------------------------------------

void genRandom(vector<PCB> & readyVector);
bool search(vector<PCB> & temp, int pid);

//------------------------------------------------------------------------------------------------------


/*
	Name:
	Authors:
	Purpose:
	Input:
	Output:
	Return:
*/
int main() {

	srand(time(NULL)); //initial seed

	int numQueues = 1;
	int time = 0;

	// Get the number of queues to simulate (1 <= N <= 5)
	cout << "Number of queues to simulate: ";
	cin >> numQueues;
	if (numQueues > MAX_QUEUES)
		numQueues = MAX_QUEUES;
	else if (numQueues < MIN_QUEUES)
		numQueues = MIN_QUEUES;
	cout << "Round Robin Time Slice: ";
	cin >> time;

	// Dynamically allocated containers
	vector<PCB> * readyQueues = new vector<PCB>[numQueues];
	SJF * mySJF = new SJF[numQueues];
	Priority * myPriority = new Priority[numQueues];
	RoundRobin * myRR = new RoundRobin[numQueues];

	// Randomly generate the queues
	for (int i = 0; i < numQueues; i++)
		genRandom(readyQueues[i]);

	// Initialize the schedulers
	for (int i = 0; i < numQueues; i++) {

		mySJF[i].setReadyQueue(readyQueues[i]);
		myPriority[i].setReadyQueue(readyQueues[i]);
		myRR[i].setReadyQueue(readyQueues[i]);

	}

	// Simulate the schedulers
	cout << endl << "Simulating . . . ";
	for (int i = 0; i < numQueues; i++) {

		mySJF[i].runSchedule();
		myPriority[i].runSchedule();
		myRR[i].runSchedule();

	}
	cout << "Done." << endl << endl;
	system("PAUSE");
	system("CLS");
	
	cout << LINE << endl;
	cout << "SJF:      |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |  Avg Turnaround" << endl;
	cout << LINE << endl;
	for (int i = 0; i < numQueues; i++)
		printf("  Q%d      |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
				i + 1,
				myPriority[i].getNumProcs(), 
				9, myPriority[i].getAvgBurst(), 
				8, mySJF[i].getAvgWait(), 
				8, mySJF[i].getAvgResp(), 
				8, mySJF[i].getAvgTurn());
	cout << LINE << endl;

	cout << "Priority: |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |  Avg Turnaround" << endl;
	cout << LINE << endl;
	for (int i = 0; i < numQueues; i++)
		printf("  Q%d      |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
				i + 1,
				myPriority[i].getNumProcs(), 
				9, myPriority[i].getAvgBurst(), 
				8, myPriority[i].getAvgWait(), 
				8, myPriority[i].getAvgResp(), 
				8, myPriority[i].getAvgTurn());
	cout << LINE << endl;

	cout << "RR:       |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |  Avg Turnaround" << endl;
	cout << LINE << endl;
	for (int i = 0; i < numQueues; i++)
		printf("  Q%d      |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
				i + 1,
				myRR[i].getNumProcs(), 
				9, myRR[i].getAvgBurst(), 
				8, myRR[i].getAvgWait(), 
				8, myRR[i].getAvgResp(), 
				8, myRR[i].getAvgTurn());
	cout << LINE << endl;

	// Returning memory
	for (int i = 0; i < numQueues; i++)
		readyQueues[i].clear();

	delete [] readyQueues;
	delete [] mySJF;
	delete [] myPriority;
	delete [] myRR;

	cout << endl;
	system("PAUSE");
	return 0;

}

//------------------------------------------------------------------------------------------------------


/*
	Name:
	Authors: Scott Smoke
	Purpose:
	Input:
	Output:
	Return:
*/
/*
	This will randomly generate the number of processes, the burst time, and pid.
	References: cplusplus.com
*/
void genRandom(vector<PCB> & readyVector) {

	srand(rand());

	int r = 0;
	//int s = (rand() % 10000) + 1;

	for (unsigned int i = 0; i < MAX_PROCS; i++) {

		srand(rand());	//reseed
		r = rand();

		if(search(readyVector, (r % PID_BOUND) + 1) == false)
			readyVector.push_back(PCB((r % PID_BOUND) + 1, (r % BURST_BOUND) + 1));
	
	}

}

//------------------------------------------------------------------------------------------------------

/*
	Name:
	Authors: Scott Smoke
	Purpose:
	Input:
	Output:
	Return:
*/
bool search(vector<PCB> & temp, int pid) {

	int i = 0;
	int vecSize = temp.size();

	for(int i = 0 ; i < vecSize; i++)
		if(pid == temp[i].getPID())
			return true;
	
	return false;

}