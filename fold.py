
import os

f = open("outline.txt", "r")
text = f.read();
f.close()

lines = text.split("\n")
startcount = 1
for x in range(0,len(lines)):
	foldername = "%02d - %s" % (startcount, lines[x])
	startcount += 1
	if not os.path.exists(foldername):
		os.mkdir(foldername)
		print foldername