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

#include <iostream>
#include <stdio.h>
#include <ctime>
#include <vector>
#include <string>
#include <fstream>

#include "Schedule.h"
#include "PCB.h"

using namespace std;

//------------------------------------------------------------------------------------------------------

const unsigned int MAX_PROCS = 500;
const unsigned int PID_BOUND = 1000;
const unsigned int BURST_BOUND = 100;

const unsigned int MAX_QUEUES = 9;
const unsigned int MIN_QUEUES = 1;

const unsigned int MAX_RR = 3;
const unsigned int MIN_RR = 1;

const string LINE = "-------------------------------------------------------------------------------";
const string LINE2 = "-------------------";

//------------------------------------------------------------------------------------------------------

int mainMenu();

bool loadFile(vector<PCB> & readyQueue);

void getInitVals(int & numQueues, int & numRR, int time[MAX_RR]);
void getInitVals(int & numRR, int time[MAX_RR]);

void doSimRandom(int numQueues, int numRR, int time[MAX_RR]);
void doSimFile(int numRR, int time[MAX_RR], const vector<PCB> & readyQueue);

void genRandom(vector<PCB> & readyVector);
bool search(const vector<PCB> & temp, int pid);

//------------------------------------------------------------------------------------------------------

int main() {

	srand(time(NULL));	// initial seed

	int numQueues = 1;
	int numRR = 1;
	int time[MAX_RR] = {0};
	vector<PCB> readyQueue;

	int sel = mainMenu();
	while (sel != 3) {

		system("CLS");
	
		// Load from file
		if (sel == 1) {

			if (loadFile(readyQueue)) {

				getInitVals(numRR, time);
				doSimFile(numRR, time, readyQueue);

				// Clear queue between loads
				readyQueue.clear();

			}
				
			else 
				cout << "ERROR!" << endl;

		}

		// Randomize
		else if (sel == 2) {

			getInitVals(numQueues, numRR, time);
			doSimRandom(numQueues, numRR, time);

		}

		system("PAUSE");
		sel = mainMenu();
		
	}
	
	return 0;

}



/*
	Name: genRandom
	Authors: Scott Smoke and Michael Beaver
	Purpose: This will randomly generate the number of processes, the burst time, and pid.
		References: cplusplus.com
	Input: readyVector is a vector of PCB objects.
	Output: readyVector is a vector of PCB objects.
	Return: N/A (void)
*/
void genRandom(vector<PCB> & readyVector) {

	srand(rand());

	int r = 0;

	for (unsigned int i = 0; i < MAX_PROCS; i++) {

		srand(rand());	//reseed
		r = rand();

		// Only add PCBs with unique PIDs
		if(search(readyVector, (r % PID_BOUND) + 1) == false)
			readyVector.push_back(PCB((r % PID_BOUND) + 1, (r % BURST_BOUND) + 1));
	
	}

}



/*
	Name: search
	Authors: Scott Smoke
	Purpose: This will search the vector that was randomly generated and ensure
			 that every PID is unique.
	Input: temp is the vector of PCBs to be checked, and pid is the PID to find.
	Output: N/A
	Return: Returns true if found, and false if not found.
*/
bool search(const vector<PCB> & temp, int pid) {

	int i = 0;
	int vecSize = temp.size();

	// Compare each PCB against the given PID
	for(int i = 0 ; i < vecSize; i++)
		if(pid == temp[i].getPID())
			return true;
	
	return false;

}



/*
	Name: getInitVals
	Authors: Michael Beaver
	Purpose: Prompts the user for initial simulation values.
	Input: numQueues is the number of queues to simulate, numRR is the number of
		Round Robin time slices to simulate, and time is an array of time slice values.
	Output: numQueues is the number of queues to simulate, numRR is the number of
		Round Robin time slices to simulate, and time is an array of time slice values.
	Return: N/A (void)
*/
void getInitVals(int & numQueues, int & numRR, int time[MAX_RR]) {

	system("CLS");

	// Getting number of queues
	cout << "Number of queues to simulate [" << MIN_QUEUES << ", " << MAX_QUEUES <<"]: ";
	cin >> numQueues;
	if (numQueues > MAX_QUEUES)
		numQueues = MAX_QUEUES;
	else if (numQueues < MIN_QUEUES)
		numQueues = MIN_QUEUES;

	// Getting number of time slices
	cout << "Number of Round Robin time slices to simulate [" << MIN_RR << ", " << MAX_RR <<"]: ";
	cin >> numRR;
	if (numRR > MAX_RR)
		numRR = MAX_RR;
	else if (numRR < MIN_RR)
		numRR = MIN_RR;

	// Getting the time slices
	cout << endl;
	for (int i = 0; i < numRR; i++) {

		cout << "Round Robin #" << i + 1 << " time slice: ";
		cin >> time[i];

	}
	cout << endl;

}



/*
	Name: getInitVals
	Authors: Michael Beaver
	Purpose: Prompts the user for initial simulation values.
	Input: numRR is the number of Round Robin time slices to simulate, and time is 
		an array of time slice values.
	Output: numRR is the number of Round Robin time slices to simulate, and time is 
		an array of time slice values.
	Return: N/A (void)
*/
void getInitVals(int & numRR, int time[MAX_RR]) {

	system("CLS");

	// Getting number of time slices
	cout << "Number of Round Robin time slices to simulate [" << MIN_RR << ", " << MAX_RR <<"]: ";
	cin >> numRR;
	if (numRR > MAX_RR)
		numRR = MAX_RR;
	else if (numRR < MIN_RR)
		numRR = MIN_RR;

	// Getting the time slices
	cout << endl;
	for (int i = 0; i < numRR; i++) {

		cout << "Round Robin #" << i + 1 << " time slice: ";
		cin >> time[i];

	}
	cout << endl;

}



/*
	Name: doSimRandom
	Authors: Michael Beaver and Scott Smoke
	Purpose: Runs the scheduler simulations and outputs a table of statistical results.
	Input: numQueues is the number of queues to simulate, numRR is the number of
		Round Robin time slices to simulate, and time is an array of time slice values.
	Output: N/A
	Return: N/A (void)
*/
void doSimRandom(int numQueues, int numRR, int time[MAX_RR]) {

	// Dynamically allocated containers
	vector<PCB> * readyQueues = new vector<PCB>[numQueues];
	SJF * mySJF = new SJF[numQueues];
	Priority * myPriority = new Priority[numQueues];
	RoundRobin * myRR = new RoundRobin[numQueues];

	// Randomly generate the queues
	for (int i = 0; i < numQueues; i++)
		genRandom(readyQueues[i]);

	// Initialize and simulate the simpler schedulers
	for (int i = 0; i < numQueues; i++) {

		mySJF[i].setReadyQueue(readyQueues[i]);
		myPriority[i].setReadyQueue(readyQueues[i]);

		mySJF[i].runSchedule();
		myPriority[i].runSchedule();

	}

	// Output Shortest Job First Results
	cout << LINE << endl;
	cout << "     SJF    |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |   Avg Turn" << endl;
	cout << LINE << endl;
	for (int i = 0; i < numQueues; i++) {

		printf("  Q%d        |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
				i + 1,
				myPriority[i].getNumProcs(), 
				9, myPriority[i].getAvgBurst(), 
				8, mySJF[i].getAvgWait(), 
				8, mySJF[i].getAvgResp(), 
				8, mySJF[i].getAvgTurn());

	}
	cout << LINE << endl;

	// Output Priority Results
	cout << "  Priority  |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |   Avg Turn" << endl;
	cout << LINE << endl;
	for (int i = 0; i < numQueues; i++) {

		printf("  Q%d        |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
				i + 1,
				myPriority[i].getNumProcs(), 
				9, myPriority[i].getAvgBurst(), 
				8, myPriority[i].getAvgWait(), 
				8, myPriority[i].getAvgResp(), 
				8, myPriority[i].getAvgTurn());

	}
	cout << LINE << endl;

	// Simulate the Round Robin cases
	for (int j = 0; j < numRR; j++) {

		// Output Round Robin Results
		cout << "  RR |  TS  |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |   Avg Turn" << endl;
		cout << LINE << endl;
		for (int i = 0; i < numQueues; i++) {

			// Simulation happens here (to accomodate numerous time slice simulations)
			myRR[i].setReadyQueue(readyQueues[i]);
			myRR[i].setTimeSlice(time[j]);
			myRR[i].runSchedule();

			printf("  Q%d | %*d |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n",
					i + 1,
					4, myRR[i].getTimeSlice(),
					myRR[i].getNumProcs(), 
					9, myRR[i].getAvgBurst(), 
					8, myRR[i].getAvgWait(), 
					8, myRR[i].getAvgResp(), 
					8, myRR[i].getAvgTurn());

			// Data must be reset after each simulation
			myRR[i].resetData();

		}

		cout << LINE << endl;

	}

	// Returning memory
	for (int i = 0; i < numQueues; i++)
		readyQueues[i].clear();

	delete [] readyQueues;
	delete [] mySJF;
	delete [] myPriority;
	delete [] myRR;

}



/*
	Name: doSimFile
	Authors: Michael Beaver
	Purpose: Runs the scheduler simulations and outputs a table of statistical results.
	Input: numRR is the number of Round Robin time slices to simulate, time is an array
		of time slice values, and readyQueue is the ready queue on which to simulate.
	Output: readyQueue is the ready queue on which to simulate.
	Return: N/A (void)
*/
void doSimFile(int numRR, int time[MAX_RR], const vector<PCB> & readyQueue) {

	SJF mySJF;
	Priority myPriority;
	RoundRobin myRR;

	// Run simpler schedules first (see below for Round Robin)
	mySJF.setReadyQueue(readyQueue);
	myPriority.setReadyQueue(readyQueue);
	mySJF.runSchedule();
	myPriority.runSchedule();

	// Output Shortest Job First Results
	cout << LINE << endl;
	cout << "     SJF    |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |   Avg Turn" << endl;
	cout << LINE << endl;
	printf("  Q%d        |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
			1,
			myPriority.getNumProcs(), 
			9, myPriority.getAvgBurst(), 
			8, mySJF.getAvgWait(), 
			8, mySJF.getAvgResp(), 
			8, mySJF.getAvgTurn());
	cout << LINE << endl;

	// Output Priority Results
	cout << "  Priority  |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |   Avg Turn" << endl;
	cout << LINE << endl;
	printf("  Q%d        |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n", 
			1,
			myPriority.getNumProcs(), 
			9, myPriority.getAvgBurst(), 
			8, myPriority.getAvgWait(), 
			8, myPriority.getAvgResp(), 
			8, myPriority.getAvgTurn());
	cout << LINE << endl;

	// Simulate the Round Robin cases (to accomodate numerous time slice simulations)
	cout << "  RR |  TS  |  # Procs  |  Avg Burst  |  Avg Wait  |  Avg Resp  |   Avg Turn" << endl;
	cout << LINE << endl;
	for (int i = 0; i < numRR; i++) {

		myRR.setReadyQueue(readyQueue);
		myRR.setTimeSlice(time[i]);
		myRR.runSchedule();

		// Output Round Robin Results
		printf("  Q%d | %*d |  %7d  |  %*.2f  |  %*.2f  |  %*.2f  |  %*.2f\n",
				1,
				4, myRR.getTimeSlice(),
				myRR.getNumProcs(), 
				9, myRR.getAvgBurst(), 
				8, myRR.getAvgWait(), 
				8, myRR.getAvgResp(), 
				8, myRR.getAvgTurn());

		// Data must be reset after each simulation
		myRR.resetData();
		
	}

	cout << LINE << endl;

}



/*
	Name: loadFile
	Authors: Michael Beaver
	Purpose: Loads ready queue data from a user-specified file.
	Input: readyQueue is the ready queue in which the data will be read.
	Output: readyQueue is the ready queue in which the data will be read.
	Return: Returns true if the file was read successfully but false if otherwise.
*/
bool loadFile(vector<PCB> & readyQueue) {

	string filename;
	ifstream myFile;
	bool flag = false;

	int pid;
	int cpuBurst;
	int state; 
	int arrival;
	int priority;

	// clearing the cin buffer: http://www.stackoverflow.com/questions/257091/how-do-i-flush-the-cin-buffer
	cin.clear();
	cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');

	cout << "File Name: ";
	getline(cin, filename);
	myFile.open(filename.c_str());

	// Read in PCB data and update ready queue
	while (myFile.is_open() && !myFile.eof()) {

		myFile >> pid;
		myFile >> state;
		myFile >> cpuBurst;
		myFile >> arrival;
		myFile >> priority;
		
		PCB proc(pid, cpuBurst, state, arrival, priority);
		readyQueue.push_back(proc);

		flag = true;

	}

	myFile.close();
	return flag;

}



/*
	Name: mainMenu
	Authors: Michael Beaver
	Purpose: Displays a main menu screen.
	Input: N/A
	Output: N/A
	Return: Returns the user's menu selection.
*/
int mainMenu() {

	int sel = 0;

	system("CLS");

	cout << LINE2 << endl;
	cout << "     Main Menu     " << endl;
	cout << LINE2 << endl;
	cout << " 1) Load from File " << endl;
	cout << " 2) Randomize		" << endl;
	cout << " 3) Quit			" << endl;
	cout << LINE2 << endl << endl;

	// Make sure the selection is valid
	while ((sel < 1) || (sel > 3)) {
	
		cout << "Selection: ";
		cin >> sel;

	}

	return sel;

}

