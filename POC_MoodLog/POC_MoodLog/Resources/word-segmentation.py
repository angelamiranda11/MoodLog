import sys
from wordsegment import segment

def segmentInput(input):
    segment(input)

input = sys.stdin.readline()
print segmentInput(input)
sys.stdout.flush()
