using System;
using System.Collections.Generic;
using System.Linq;
using Template.Modules.Utils;

namespace Template.Modules.Mazzin;

public class GoalList
{
    internal int NumberOfGoals {get; set; }
    
    internal List<Goal> Goals {get; set; }
    
    
    private HashSet<Goal> _goals;
    
    public GoalList()
    {
        _goals = new HashSet<Goal>();
        Goals = new List<Goal>();
        NumberOfGoals = 0;
    }

    public GoalList(IEnumerable<Goal> goals)
    {
        _goals = new HashSet<Goal>();
        Goals = new List<Goal>();
        NumberOfGoals = 0;
        AddAll(goals);
    }

    public GoalList(string dataLocation)
    {
        DataAssociation data = DataAssociation.FromFile(dataLocation);
        Dictionary<string, string>.ValueCollection dataStringDataCollection = data.Values;
        List<string> dataStringData = dataStringDataCollection.ToList();
        _goals = new HashSet<Goal>();
        Goals = new List<Goal>();
        NumberOfGoals = 0;
        AddAll(dataStringData);
    }
    
    public bool Add(Goal goal)
    {
        if (_goals.Contains(goal)) return false;
        
        _goals.Add(goal);
        Goals.Add(goal);
        NumberOfGoals++;
        return true;
    }

    public bool Add(string goal)
    {
        Goal newGoal = Goal.Parse(goal);
        return Add(newGoal);
    }
    
    public void AddAll(IEnumerable<Goal> goals)
    {
        foreach (Goal goal in goals)
        {
            Add(goal);
        }
    }
    
    public void AddAll(List<string> goals)
    {
        foreach (string goal in goals)
        {
            Add(goal);
        }
    }
    
    public bool ContainsGoal(Goal goal)
    {
        return _goals.Contains(goal);
    }
    
    public bool ContainsGoal(string goal)
    {
        Goal newGoal = Goal.Parse(goal);
        return ContainsGoal(newGoal);
    }
    
    public bool Remove(Goal goal)
    {
        if (!_goals.Contains(goal)) return false;
        
        _goals.Remove(goal);
        Goals.Remove(goal);
        NumberOfGoals--;
        return true;
    }
    
    public bool Remove(string goal)
    {
        Goal newGoal = Goal.Parse(goal);
        return Remove(newGoal);
    }
    
    public void RemoveAll(IEnumerable<Goal> goals)
    {
        foreach (Goal goal in goals)
        {
            Remove(goal);
        }
    }
    
    public void RemoveAll(List<string> goals)
    {
        foreach (string goal in goals)
        {
            Remove(goal);
        }
    }
    
    public Goal RandomGoal()
    {
        if (NumberOfGoals == 0) return null;
        Random random = new();
        int randomIndex = random.Next(0, NumberOfGoals);
        return Goals[randomIndex];
    }
    
    public bool IsEmpty => NumberOfGoals == 0;
    
    internal void Clear()
    {
        _goals.Clear();
        Goals.Clear();
        NumberOfGoals = 0;
    }

    internal bool Save(string location)
    {
        DataAssociation data = DataAssociation.FromFile(location);
        foreach (Goal goal in Goals)
        {
            string goalName = goal.Name;
            string goalData = goal.ToStringFull();
            data.AddProprety(goalName, goalData);
        }
        
        return data.Save();
    }
}