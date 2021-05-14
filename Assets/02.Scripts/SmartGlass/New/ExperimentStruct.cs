using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Distance
{
    Near,
    Far
}

enum Technique
{
    PhoneSwipe,
    PhoneGyro,
    GlassesHand
}

struct Block
{
    int number;
    Distance distance;
    Technique technique;
}

struct ExperimentState
{
    int participantNum;
    int curBlockNum;
    Distance curBlockDistance;
    Technique curBlockTechnique;
    float curPositionOffset;
    float curRotationOffset;
}

struct ExperimentFlow
{
    int flowNum;
    List<Block> blockOrder;
}

struct ExperimentRandomValue
{
    List<float> NearPositionOffsetValues;
    List<float> FarPositionOffsetValues;
    List<float> RotationOffsetValues;
}