#include <stdio.h>
#include <sys/time.h>
#include <sys/types.h>
#include <unistd.h>

int main(void) {

	fd_set rfds;
	struct timeval tv;
	int retval;

	FD_ZERO(&rfds);
	FD_SET(0, &rfds);
	tv.tv_sec = 5;
	tv.tv_usec = 0;
	
	retval = select(1, &rfds, NULL, NULL, &tv);

	if (retval == -1)
		perror("Goofed up\n");
	else if (retval)
		printf("Data available on keyboard\n");
	else
		printf("No data in last 5 seconds\n");

	return 0;

}
