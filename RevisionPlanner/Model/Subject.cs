﻿namespace RevisionPlanner.Model;

public class Subject
{
    public uint ID { get; set; }

    public string ExamBoard { get; set; }

    public string Qualification { get; set; }

    //public Color Color { get; set; }

    public Topic[] Topics { get; set; }
}
