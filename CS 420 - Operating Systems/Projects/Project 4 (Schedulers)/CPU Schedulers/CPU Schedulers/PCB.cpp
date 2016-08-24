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

#include "PCB.h"

/*
	Name: PCB in scope of PCB
	Authors: Scott Smoke
	Purpose: Default Constructor. All variables are initialized to default values.
	Input: N/A
	Output: N/A
	Return: N/A
*/
PCB::PCB() : pid(1), proState(0), cpuTime(0), arrTime(0), priority(0), 
			waitTime(0), respTime(0), turnTime(0), remBurst(0), executionTime(0) { }



/*
	Name: PCB in scope of PCB
	Authors: Scott Smoke
	Purpose: Overloaded Constructor. Process ID and CPU Burst Time may
		be set, but all other values are default.
	Input: id is the Process ID, and cpuBurst is the CPU Burst Time.
	Output: N/A
	Return: N/A
*/
PCB::PCB(int id, int cpuBurst) {

	pid = id;
	proState = 0;		//initializes to Ready
	cpuTime = cpuBurst;
	arrTime = -1;
	priority = 0;
	waitTime = 0;
	respTime = 0;
	turnTime = 0;
	executionTime = cpuBurst;
	remBurst = cpuBurst;

}



/*
	Name: PCB in scope of PCB
	Authors: Michael Beaver
	Purpose: Overloaded Constructor. Process ID, CPU Burst Time, Process State,
		Arrival Time, and Priority may be set, but all other values are default.
	Input: id is the Process ID, cpuBurst is the CPU Burst Time, state is the
		Process State, arrival is the arrival time, and procPriority is the
		process' priority.
	Output: N/A	
	Return: N/A
*/
PCB::PCB(int id, int cpuBurst, int state, int arrival, int procPriority) {

	pid = id;
	proState = state;		
	cpuTime = cpuBurst;
	arrTime = arrival;
	priority = procPriority;
	waitTime = 0;
	respTime = 0;
	turnTime = 0;
	remBurst = cpuBurst;
	executionTime = cpuBurst;
	
}



/*
	Name: PCB in scope of PCB
	Authors: Scott Smoke
	Purpose: Copy Constructor. The values of one PCB are copied to this PCB.
	Input: right is the PCB whose values are to be copied.
	Output: right is the PCB whose values are to be copied.
	Return: N/A
*/
PCB::PCB(const PCB & right) {

	pid = right.pid;
	proState = right.proState;  
	cpuTime = right.cpuTime;
	arrTime = right.arrTime;
	priority = right.priority;
	waitTime = right.waitTime;
	respTime = right.respTime;
	turnTime = right.turnTime;
	remBurst = right.remBurst;
	executionTime = right.executionTime;

}



/*
	Name: operator = in scope of PCB
	Authors: Scott Smoke
	Purpose: Overloaded Assignment Operator. The values of a PCB
		are assigned to this PCB.
	Input: right is the PCB whose values are to be copied.
	Output: right is the PCB whose values are to be copied.
	Return: Returns a reference to this PCB.
*/
PCB & PCB::operator =(const PCB & right) {

	// Check for self-assignment
	if (this == &right) 
		return *this;
	
	pid = right.pid;
	proState = right.proState;  
	cpuTime = right.cpuTime;
	arrTime = right.arrTime;
	priority = right.priority;
	waitTime = right.waitTime;
	respTime = right.respTime;
	turnTime = right.turnTime;
	executionTime = right.executionTime;
	remBurst = right.remBurst;

	return *this;

}



/*
	Name: getProState in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' state.
	Input: N/A
	Output: N/A
	Return: Returns the process' state.
*/
int PCB::getProState() const { 
	
	return proState; 

}



/*
	Name: getPID in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' ID.
	Input: N/A
	Output: N/A
	Return: Returns the process' ID.
*/
int PCB::getPID() const	{ 
	
	return pid; 

}



/*
	Name: getArrTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' arrival time.
	Input: N/A
	Output: N/A
	Return: Returns the process' arrival time.
*/
int PCB::getArrTime() const	{ 
	
	return arrTime; 

}



/*
	Name: getWaitTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' wait time.
	Input: N/A
	Output: N/A
	Return: Returns the process' wait time.
*/
int PCB::getWaitTime() const { 
	
	return waitTime; 

}



/*
	Name: getRespTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' response time.
	Input: N/A
	Output: N/A
	Return: Returns the process' response time.
*/
int PCB::getRespTime() const { 
	
	return respTime; 

}



/*
	Name: getTurnTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' turnaround time.
	Input: N/A
	Output: N/A
	Return: Returns the process' turnaround time.
*/
int PCB::getTurnTime() const { 
	
	return turnTime; 

}



/*
	Name: getBurstTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' CPU Burst Time.
	Input: N/A
	Output: N/A
	Return: Returns the process' CPU Burst Time.
*/
int PCB::getBurstTime() const { 
	
	return cpuTime; 

}



/*
	Name: getRemBurst in scope of PCB
	Authors: Michael Beaver
	Purpose: Returns the process' remaining CPU Burst Time.
	Input: N/A
	Output: N/A
	Return: Returns the process' remaining CPU Burst Time.
*/
int PCB::getRemBurst() const { 
	
	return remBurst; 

}



/*
	Name: getPriority in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' priority.
	Input: N/A
	Output: N/A
	Return: Returns the process' priority.
*/
int PCB::getPriority() const  { 
	
	return priority; 

}



/*
	Name: getExecution in scope of PCB
	Authors: Scott Smoke
	Purpose: Returns the process' execution time.
	Input: N/A
	Output: N/A
	Return: Returns the process' execution time.
*/
int PCB::getExecution() const {
	
	return executionTime;

}



/*
	Name: setPID in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' ID.
	Input: newPID is the new process ID.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setPID(int newPID) { 
	
	pid = newPID; 

}



/*
	Name: setCPUTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' CPU Burst Time.
	Input: cpuBurst is the new CPU Burst Time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setCPUTime(int cpuBurst) {
	
	cpuTime = cpuBurst; 

}



/*
	Name: setArrTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' arrival time.
	Input: arrival is the new arrival time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setArrTime(int arrival) {
	
	arrTime = arrival; 

} 



/*
	Name: setProState in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' state.
	Input: state is the new state.
	Output: N/A 
	Return: Returns true if the state change was successful; otherwise, the
		process is put in the waiting state and returns false.
*/
bool PCB::setProState(int state) {
	
	// -2 => Terminated; -1 => Waiting; 0 => Ready; 1 => Running
	if (state == -2 || state == -1 || state == 0 || state == 1) {

		proState = state;
		return true;

	}

	// Go to waiting state
	state = -1;
	return false;

}



/*
	Name: setWaitTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' wait time.
	Input: wait is the new wait time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setWaitTime(int wait)	{
	
	waitTime = wait; 

}



/*
	Name: setRespTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' response time.
	Input: resp is the new response time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setRespTime(int resp)	{
	
	respTime = resp; 

}



/*
	Name: setTurnTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' turnaround time.
	Input: turn is the new turnaround time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setTurnTime(int turn)	{
	
	turnTime = turn;

}



/*
	Name: setPriority in scope of PCB
	Authors: Michael Beaver
	Purpose: Sets the process' priority.
	Input: nPriority is the new priority.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setPriority(int nPriority) {
	
	priority = nPriority;

}



/*
	Name: setRemBurst in scope of PCB
	Authors: Michael Beaver
	Purpose: Sets the process' remaining CPU Burst Time.
	Input: burst is the new remaining CPU Burst Time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setRemBurst(int burst) {
	
	remBurst = burst;

}



/*
	Name: setExecutionTime in scope of PCB
	Authors: Scott Smoke
	Purpose: Sets the process' execution time.
	Input: exec is the new execution time.
	Output: N/A 
	Return: N/A (void)
*/
void PCB::setExecutionTime(int exec) {
	
	executionTime = exec; 

}



/*
	Name: operator < in scope of PCB
	Authors: Scott Smoke
	Purpose: Overloaded Less Than Operator. Compares one process' priority
		against another process' priority.
	Input: lhs is the left-hand PCB, and rhs is the right-hand PCB.
	Output: lhs is the left-hand PCB, and rhs is the right-hand PCB.
	Return: Returns true is the left-hand process' priority is greater than
		the right-hand process' priority.
*/
bool operator <(const PCB & lhs, const PCB & rhs) {
	
	return (lhs.priority > rhs.priority); 

}

