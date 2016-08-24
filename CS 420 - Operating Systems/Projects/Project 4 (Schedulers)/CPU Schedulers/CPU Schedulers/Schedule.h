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

#ifndef SCHEDULE_H
#define SCHEDULE_H

#include <vector>
#include <queue>

#include "PCB.h"

// Michael Beaver
class Schedule {

	protected:
		std::vector<PCB> readyVector;
		std::queue<PCB> readyQueue;

		float avgWait;
		float avgResp;
		float avgTurn;
		float avgBurst;
		int totalTime;

		void calcArrTime();
		void calcWaitTime();	// wait = time elapsed
		void calcRespTime();	// response = arrival + wait
		void calcTurnTime();	// turnaround = wait + cpu burst + arrival
		void calcBurstTime();

	public:
		virtual void runSchedule() = 0;

		float getAvgWait() const;
		float getAvgResp() const;
		float getAvgTurn() const;
		float getAvgBurst() const;
		int getNumProcs() const;
		int getTotalTime() const;

		virtual void setReadyQueue(const std::vector<PCB> & newVector);

};



// Wenhao Wang
class SJF : public Schedule {

	public:
		SJF();
		SJF(const std::vector<PCB> & newQueue);

		void runSchedule();

		//void setReadyQueue(const std::vector<PCB> & newVector);

};



// Scott Smoke
class Priority : public Schedule {

	private:
		int numOfProcesses;
		int totalWait;
		int totalTurn;
		int totalBurst;

		void genRandomPriority();

	public:
		Priority();
		Priority(const std::vector<PCB> & readyQueue);

		void report();
		void runSchedule();

		void setReadyQueue(const std::vector<PCB> & newVector);
		
};



// Michael Beaver
class RoundRobin : public Schedule {

	private:
		unsigned int timeSlice;

	public:
		RoundRobin();
		RoundRobin(const std::queue<PCB> & newQueue);
		RoundRobin(const std::vector<PCB> & newQueue);
		RoundRobin(const std::queue<PCB> & newQueue, unsigned int time);
		RoundRobin(const std::vector<PCB> & newQueue, unsigned int time);

		void runSchedule();
		
		void setReadyQueue(const std::vector<PCB> & newVector);
		void setReadyQueue(const std::queue<PCB> & newQueue);
		void setTimeSlice(unsigned int time);		
		void resetData();

		unsigned int getTimeSlice() const;

};

#endif

