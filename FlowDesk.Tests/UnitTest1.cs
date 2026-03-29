using FlowDesk.Domain.Entities;
using FlowDesk.Domain.Enums;
using FluentAssertions;

namespace FlowDesk.Tests;

public class TaskItemTransitionTests
{
    [Fact]
    public void ToDo_Can_Transition_To_InProgress()
    {
        var task = new TaskItem { Status = BoardTaskStatus.ToDo };
        task.TransitionTo(BoardTaskStatus.InProgress);
        task.Status.Should().Be(BoardTaskStatus.InProgress);
    }

    [Fact]
    public void ToDo_Cannot_Transition_To_Done()
    {
        var task = new TaskItem { Status = BoardTaskStatus.ToDo };
        var act = () => task.TransitionTo(BoardTaskStatus.Done);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ToDo_Cannot_Transition_To_Archived()
    {
        var task = new TaskItem { Status = BoardTaskStatus.ToDo };
        var act = () => task.TransitionTo(BoardTaskStatus.Archived);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void InProgress_Can_Transition_To_Done()
    {
        var task = new TaskItem { Status = BoardTaskStatus.InProgress };
        task.TransitionTo(BoardTaskStatus.Done);
        task.Status.Should().Be(BoardTaskStatus.Done);
    }

    [Fact]
    public void InProgress_Can_Go_Back_To_ToDo()
    {
        var task = new TaskItem { Status = BoardTaskStatus.InProgress };
        task.TransitionTo(BoardTaskStatus.ToDo);
        task.Status.Should().Be(BoardTaskStatus.ToDo);
    }

    [Fact]
    public void Done_Can_Transition_To_Archived()
    {
        var task = new TaskItem { Status = BoardTaskStatus.Done };
        task.TransitionTo(BoardTaskStatus.Archived);
        task.Status.Should().Be(BoardTaskStatus.Archived);
    }

    [Fact]
    public void Archived_Cannot_Transition_To_Anything()
    {
        var task = new TaskItem { Status = BoardTaskStatus.Archived };
        var act = () => task.TransitionTo(BoardTaskStatus.ToDo);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Transition_Updates_UpdatedAt_Timestamp()
    {
        var task = new TaskItem { Status = BoardTaskStatus.ToDo };
        var before = task.UpdatedAt;
        task.TransitionTo(BoardTaskStatus.InProgress);
        task.UpdatedAt.Should().BeOnOrAfter(before);
    }
}