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

#ifndef PCB_H
#define PCB_H

class PCB {

	private:
		int pid;			// process ID
		int proState;		// state of the process: -2 = Terminated; -1 = Waiting; 0 = Ready; 1 = Running
		int cpuTime;		// CPU Burst Time
		int arrTime;		// arrival time
		int waitTime;		// calculated: wait = time elapsed
		int respTime;		// calculated: response = arrival + wait
		int turnTime;		// calculated: turnaround = wait + cpu burst + arrival
		int remBurst;		// Remaining burst time, for Round Robin scheduler
		int executionTime;  // mutatable variable of burst time, for Priority scheduler
		int priority;		// Process priority

	public:
		PCB();
		PCB(int id, int cputBurst);
		PCB(int id, int cpuBurst, int state, int arrival, int procPriority);
		PCB(const PCB & right);

		int getProState() const;
		int getPID() const;
		int getArrTime() const;
		int getWaitTime() const;
		int getRespTime() const;
		int getTurnTime() const;
		int getBurstTime() const;
		int getRemBurst() const;
		int getPriority() const;
		int getExecution() const;

		void setPID(int newPID);
		void setCPUTime(int cpuBurst);
		void setArrTime(int arrival); 
		bool setProState(int state);
		void setWaitTime(int wait);
		void setRespTime(int resp);
		void setTurnTime(int turn);
		void setPriority(int nPriority);
		void setRemBurst(int burst);
		void setExecutionTime(int exec);

		PCB & operator =(const PCB & rhs);
		friend bool operator <(const PCB & lhs, const PCB & rhs);

};

#endif

