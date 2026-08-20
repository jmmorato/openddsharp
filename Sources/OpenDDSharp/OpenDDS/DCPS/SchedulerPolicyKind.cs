namespace OpenDDSharp.OpenDDS.DCPS;

/// <summary>
/// Defines the scheduler policy kind enumeration.
/// </summary>
[System.Flags]
public enum SchedulerPolicyKind
{
    /// <summary>
    /// Represents a scheduling policy where threads or tasks are executed in a
    /// first-in, first-out (FIFO) order. Under this policy, tasks are processed
    /// in the exact order of their arrival without prioritization or reordering.
    /// </summary>
    Fifo = 0x00020000,

    /// <summary>
    /// Represents a scheduling policy where threads or tasks are executed
    /// in a cyclic order, distributing processing time evenly among them.
    /// This policy ensures fairness by giving each task or thread an equal
    /// share of execution opportunities over time.
    /// </summary>
    RoundRobin = 0x00040000,

    /// <summary>
    /// Represents the default scheduling policy, which allows the system to determine
    /// the most appropriate scheduling approach based on the current context or configuration.
    /// This policy provides flexibility by delegating the scheduling decision to the underlying system.
    /// </summary>
    Default = 0x00080000,
}