﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using GitHub.DistributedTask.Pipelines.ContextData;
using GitHub.DistributedTask.WebApi;
using GitHub.Runner.Worker;
using Moq;
using Xunit;
using GitHub.DistributedTask.ObjectTemplating.Tokens;
using Pipelines = GitHub.DistributedTask.Pipelines;

namespace GitHub.Runner.Common.Tests.Worker
{
    public sealed class ExecutionContextL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void AddIssue_CountWarningsErrors()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));

                hc.EnqueueInstance(pagingLogger.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);

                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });

                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });

                ec.Complete();

                // Assert.
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.ErrorCount == 15)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.WarningCount == 14)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Type == IssueType.Error).Count() == 10)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Type == IssueType.Warning).Count() == 10)), Times.AtLeastOnce);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ApplyContinueOnError_CheckResultAndOutcome()
        {
            using (TestHostContext hc = CreateTestContext())
            {

                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();
                jobRequest.Variables["ACTIONS_STEP_DEBUG"] = "true";

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));
                jobServerQueue.Setup(x => x.QueueWebConsoleLine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<long>())).Callback((Guid id, string msg, long? lineNumber) => { hc.GetTrace().Info(msg); });

                hc.EnqueueInstance(pagingLogger.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);

                foreach (var tc in new List<(TemplateToken token, TaskResult result, TaskResult? expectedResult, TaskResult? expectedOutcome)> {
                (token: new BooleanToken(null, null, null, true), result: TaskResult.Failed, expectedResult: TaskResult.Succeeded, expectedOutcome: TaskResult.Failed),
                (token: new BooleanToken(null, null, null, true), result: TaskResult.Succeeded, expectedResult: TaskResult.Succeeded, expectedOutcome: null),
                (token: new BooleanToken(null, null, null, true), result: TaskResult.Canceled, expectedResult: TaskResult.Canceled, expectedOutcome: null),
                (token: new BooleanToken(null, null, null, false), result: TaskResult.Failed, expectedResult: TaskResult.Failed, expectedOutcome: null),
                (token: new BooleanToken(null, null, null, false), result: TaskResult.Succeeded, expectedResult: TaskResult.Succeeded, expectedOutcome: null),
                (token: new BooleanToken(null, null, null, false), result: TaskResult.Canceled, expectedResult: TaskResult.Canceled, expectedOutcome: null),
            })
                {
                    ec.Result = tc.result;
                    ec.Outcome = null;
                    ec.ApplyContinueOnError(tc.token);
                    Assert.Equal(ec.Result, tc.expectedResult);
                    Assert.Equal(ec.Outcome, tc.expectedOutcome);
                }
            }
        }


        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void AddIssue_TrimMessageSize()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));

                hc.EnqueueInstance(pagingLogger.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);

                var bigMessage = "";
                for (var i = 0; i < 5000; i++)
                {
                    bigMessage += "a";
                }

                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = bigMessage });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = bigMessage });
                ec.AddIssue(new Issue() { Type = IssueType.Notice, Message = bigMessage });

                ec.Complete();

                // Assert.
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Type == IssueType.Error).Single().Message.Length <= 4096)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Type == IssueType.Warning).Single().Message.Length <= 4096)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Type == IssueType.Notice).Single().Message.Length <= 4096)), Times.AtLeastOnce);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void AddIssue_AddStepAndLineNumberInformation()
        {
            using (TestHostContext hc = CreateTestContext())
            {

                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var pagingLogger2 = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));

                hc.EnqueueInstance(pagingLogger.Object);
                hc.EnqueueInstance(pagingLogger2.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);
                ec.InitializeJob(jobRequest, CancellationToken.None);
                ec.Start();

                var embeddedStep = ec.CreateChild(Guid.NewGuid(), "action_1_pre", "action_1_pre", null, null, ActionRunStage.Main, isEmbedded: true);
                embeddedStep.Start();

                embeddedStep.AddIssue(new Issue() { Type = IssueType.Error, Message = "error annotation that should have step and line number information" });
                embeddedStep.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning annotation that should have step and line number information" });
                embeddedStep.AddIssue(new Issue() { Type = IssueType.Notice, Message = "notice annotation that should have step and line number information" });

                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Data.ContainsKey("stepNumber") && i.Data.ContainsKey("logFileLineNumber") && i.Type == IssueType.Error).Count() == 1)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Data.ContainsKey("stepNumber") && i.Data.ContainsKey("logFileLineNumber") && i.Type == IssueType.Warning).Count() == 1)), Times.AtLeastOnce);
                jobServerQueue.Verify(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.Is<TimelineRecord>(t => t.Issues.Where(i => i.Data.ContainsKey("stepNumber") && i.Data.ContainsKey("logFileLineNumber") && i.Type == IssueType.Notice).Count() == 1)), Times.AtLeastOnce);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void Debug_Multilines()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();
                jobRequest.Variables["ACTIONS_STEP_DEBUG"] = "true";

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));
                jobServerQueue.Setup(x => x.QueueWebConsoleLine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<long>())).Callback((Guid id, string msg, long? lineNumber) => { hc.GetTrace().Info(msg); });

                hc.EnqueueInstance(pagingLogger.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);

                ec.Debug(null);
                ec.Debug("");
                ec.Debug("\n");
                ec.Debug("\r\n");
                ec.Debug("test");
                ec.Debug("te\nst");
                ec.Debug("te\r\nst");

                ec.Complete();

                jobServerQueue.Verify(x => x.QueueWebConsoleLine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<long?>()), Times.Exactly(10));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void RegisterPostJobAction_ShareState()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();
                jobRequest.Variables["ACTIONS_STEP_DEBUG"] = "true";

                // Arrange: Setup the paging logger.
                var pagingLogger1 = new Mock<IPagingLogger>();
                var pagingLogger2 = new Mock<IPagingLogger>();
                var pagingLogger3 = new Mock<IPagingLogger>();
                var pagingLogger4 = new Mock<IPagingLogger>();
                var pagingLogger5 = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));
                jobServerQueue.Setup(x => x.QueueWebConsoleLine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<long?>())).Callback((Guid id, string msg, long? lineNumber) => { hc.GetTrace().Info(msg); });

                var actionRunner1 = new ActionRunner();
                actionRunner1.Initialize(hc);
                var actionRunner2 = new ActionRunner();
                actionRunner2.Initialize(hc);

                hc.EnqueueInstance(pagingLogger1.Object);
                hc.EnqueueInstance(pagingLogger2.Object);
                hc.EnqueueInstance(pagingLogger3.Object);
                hc.EnqueueInstance(pagingLogger4.Object);
                hc.EnqueueInstance(pagingLogger5.Object);
                hc.EnqueueInstance(actionRunner1 as IActionRunner);
                hc.EnqueueInstance(actionRunner2 as IActionRunner);
                hc.SetSingleton(jobServerQueue.Object);

                var jobContext = new Runner.Worker.ExecutionContext();
                jobContext.Initialize(hc);

                // Act.
                jobContext.InitializeJob(jobRequest, CancellationToken.None);

                var action1 = jobContext.CreateChild(Guid.NewGuid(), "action_1", "action_1", null, null, 0);
                action1.IntraActionState["state"] = "1";
                var action2 = jobContext.CreateChild(Guid.NewGuid(), "action_2", "action_2", null, null, 0);
                action2.IntraActionState["state"] = "2";


                var postRunner1 = hc.CreateService<IActionRunner>();
                postRunner1.Action = new Pipelines.ActionStep() { Id = Guid.NewGuid(), Name = "post1", DisplayName = "Test 1", Reference = new Pipelines.RepositoryPathReference() { Name = "actions/action" } };
                postRunner1.Stage = ActionRunStage.Post;
                postRunner1.Condition = "always()";
                postRunner1.DisplayName = "post1";


                var postRunner2 = hc.CreateService<IActionRunner>();
                postRunner2.Action = new Pipelines.ActionStep() { Id = Guid.NewGuid(), Name = "post2", DisplayName = "Test 2", Reference = new Pipelines.RepositoryPathReference() { Name = "actions/action" } };
                postRunner2.Stage = ActionRunStage.Post;
                postRunner2.Condition = "always()";
                postRunner2.DisplayName = "post2";

                action1.RegisterPostJobStep(postRunner1);
                action2.RegisterPostJobStep(postRunner2);

                Assert.NotNull(jobContext.JobSteps);
                Assert.NotNull(jobContext.PostJobSteps);
                Assert.Null(action1.JobSteps);
                Assert.Null(action2.JobSteps);
                Assert.Null(action1.PostJobSteps);
                Assert.Null(action2.PostJobSteps);

                var post1 = jobContext.PostJobSteps.Pop();
                var post2 = jobContext.PostJobSteps.Pop();

                Assert.Equal("post2", (post1 as IActionRunner).Action.Name);
                Assert.Equal("post1", (post2 as IActionRunner).Action.Name);

                Assert.Equal(ActionRunStage.Post, (post1 as IActionRunner).Stage);
                Assert.Equal(ActionRunStage.Post, (post2 as IActionRunner).Stage);

                Assert.Equal("always()", (post1 as IActionRunner).Condition);
                Assert.Equal("always()", (post2 as IActionRunner).Condition);

                Assert.Equal("2", (post1 as IActionRunner).ExecutionContext.IntraActionState["state"]);
                Assert.Equal("1", (post2 as IActionRunner).ExecutionContext.IntraActionState["state"]);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void RegisterPostJobAction_NotRegisterPostTwice()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();
                jobRequest.Variables["ACTIONS_STEP_DEBUG"] = "true";

                // Arrange: Setup the paging logger.
                var pagingLogger1 = new Mock<IPagingLogger>();
                var pagingLogger2 = new Mock<IPagingLogger>();
                var pagingLogger3 = new Mock<IPagingLogger>();
                var pagingLogger4 = new Mock<IPagingLogger>();
                var pagingLogger5 = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));
                jobServerQueue.Setup(x => x.QueueWebConsoleLine(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<long?>())).Callback((Guid id, string msg, long? lineNumber) => { hc.GetTrace().Info(msg); });

                var actionRunner1 = new ActionRunner();
                actionRunner1.Initialize(hc);
                var actionRunner2 = new ActionRunner();
                actionRunner2.Initialize(hc);

                hc.EnqueueInstance(pagingLogger1.Object);
                hc.EnqueueInstance(pagingLogger2.Object);
                hc.EnqueueInstance(pagingLogger3.Object);
                hc.EnqueueInstance(pagingLogger4.Object);
                hc.EnqueueInstance(pagingLogger5.Object);
                hc.EnqueueInstance(actionRunner1 as IActionRunner);
                hc.EnqueueInstance(actionRunner2 as IActionRunner);
                hc.SetSingleton(jobServerQueue.Object);

                var jobContext = new Runner.Worker.ExecutionContext();
                jobContext.Initialize(hc);

                // Act.
                jobContext.InitializeJob(jobRequest, CancellationToken.None);

                var action1 = jobContext.CreateChild(Guid.NewGuid(), "action_1_pre", "action_1_pre", null, null, 0);
                var action2 = jobContext.CreateChild(Guid.NewGuid(), "action_1_main", "action_1_main", null, null, 0);

                var actionId = Guid.NewGuid();
                var postRunner1 = hc.CreateService<IActionRunner>();
                postRunner1.Action = new Pipelines.ActionStep() { Id = actionId, Name = "post1", DisplayName = "Test 1", Reference = new Pipelines.RepositoryPathReference() { Name = "actions/action" } };
                postRunner1.Stage = ActionRunStage.Post;
                postRunner1.Condition = "always()";
                postRunner1.DisplayName = "post1";


                var postRunner2 = hc.CreateService<IActionRunner>();
                postRunner2.Action = new Pipelines.ActionStep() { Id = actionId, Name = "post2", DisplayName = "Test 2", Reference = new Pipelines.RepositoryPathReference() { Name = "actions/action" } };
                postRunner2.Stage = ActionRunStage.Post;
                postRunner2.Condition = "always()";
                postRunner2.DisplayName = "post2";

                action1.RegisterPostJobStep(postRunner1);
                action2.RegisterPostJobStep(postRunner2);

                Assert.NotNull(jobContext.JobSteps);
                Assert.NotNull(jobContext.PostJobSteps);
                Assert.Equal(1, jobContext.PostJobSteps.Count);
                var post1 = jobContext.PostJobSteps.Pop();

                Assert.Equal("post1", (post1 as IActionRunner).Action.Name);

                Assert.Equal(ActionRunStage.Post, (post1 as IActionRunner).Stage);

                Assert.Equal("always()", (post1 as IActionRunner).Condition);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void ActionResult_Lowercase()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();
                jobRequest.Variables["ACTIONS_STEP_DEBUG"] = "true";

                // Arrange: Setup the paging logger.
                var pagingLogger1 = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                hc.EnqueueInstance(pagingLogger1.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var jobContext = new Runner.Worker.ExecutionContext();
                jobContext.Initialize(hc);

                // Act.
                jobContext.InitializeJob(jobRequest, CancellationToken.None);

                jobContext.Global.StepsContext.SetConclusion(null, "step1", ActionResult.Success);
                var conclusion1 = (jobContext.Global.StepsContext.GetScope(null)["step1"] as DictionaryContextData)["conclusion"].ToString();
                Assert.Equal(conclusion1, conclusion1.ToLowerInvariant());

                jobContext.Global.StepsContext.SetOutcome(null, "step2", ActionResult.Cancelled);
                var outcome1 = (jobContext.Global.StepsContext.GetScope(null)["step2"] as DictionaryContextData)["outcome"].ToString();
                Assert.Equal(outcome1, outcome1.ToLowerInvariant());

                jobContext.Global.StepsContext.SetConclusion(null, "step3", ActionResult.Failure);
                var conclusion2 = (jobContext.Global.StepsContext.GetScope(null)["step3"] as DictionaryContextData)["conclusion"].ToString();
                Assert.Equal(conclusion2, conclusion2.ToLowerInvariant());

                jobContext.Global.StepsContext.SetOutcome(null, "step4", ActionResult.Skipped);
                var outcome2 = (jobContext.Global.StepsContext.GetScope(null)["step4"] as DictionaryContextData)["outcome"].ToString();
                Assert.Equal(outcome2, outcome2.ToLowerInvariant());

                jobContext.JobContext.Status = ActionResult.Success;
                Assert.Equal(jobContext.JobContext["status"].ToString(), jobContext.JobContext["status"].ToString().ToLowerInvariant());
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void PublishStepTelemetry_RegularStep_NoOpt()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));

                hc.EnqueueInstance(pagingLogger.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);
                ec.Start();

                ec.Complete();

                // Assert.
                Assert.Equal(0, ec.Global.StepsTelemetry.Count);
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void PublishStepTelemetry_RegularStep()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));

                hc.EnqueueInstance(pagingLogger.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);
                ec.Start();

                ec.StepTelemetry.Type = "node16";
                ec.StepTelemetry.Action = "actions/checkout";
                ec.StepTelemetry.Ref = "v2";
                ec.StepTelemetry.IsEmbedded = false;
                ec.StepTelemetry.StepId = Guid.NewGuid();
                ec.StepTelemetry.Stage = "main";

                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Notice, Message = "notice" });
                ec.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                ec.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                ec.AddIssue(new Issue() { Type = IssueType.Notice, Message = "notice" });

                ec.Complete();

                // Assert.
                Assert.Equal(1, ec.Global.StepsTelemetry.Count);
                Assert.Equal("node16", ec.Global.StepsTelemetry.Single().Type);
                Assert.Equal("actions/checkout", ec.Global.StepsTelemetry.Single().Action);
                Assert.Equal("v2", ec.Global.StepsTelemetry.Single().Ref);
                Assert.Equal(TaskResult.Succeeded, ec.Global.StepsTelemetry.Single().Result);
                Assert.NotNull(ec.Global.StepsTelemetry.Single().ExecutionTimeInSeconds);
                Assert.Equal(3, ec.Global.StepsTelemetry.Single().ErrorMessages.Count);
                Assert.DoesNotContain(ec.Global.StepsTelemetry.Single().ErrorMessages, x => x.Contains("notice"));
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void PublishStepTelemetry_EmbeddedStep()
        {
            using (TestHostContext hc = CreateTestContext())
            {
                // Arrange: Create a job request message.
                TaskOrchestrationPlanReference plan = new TaskOrchestrationPlanReference();
                TimelineReference timeline = new TimelineReference();
                Guid jobId = Guid.NewGuid();
                string jobName = "some job name";
                var jobRequest = new Pipelines.AgentJobRequestMessage(plan, timeline, jobId, jobName, jobName, null, null, null, new Dictionary<string, VariableValue>(), new List<MaskHint>(), new Pipelines.JobResources(), new Pipelines.ContextData.DictionaryContextData(), new Pipelines.WorkspaceOptions(), new List<Pipelines.ActionStep>(), null, null, null, null);
                jobRequest.Resources.Repositories.Add(new Pipelines.RepositoryResource()
                {
                    Alias = Pipelines.PipelineConstants.SelfAlias,
                    Id = "github",
                    Version = "sha1"
                });
                jobRequest.ContextData["github"] = new Pipelines.ContextData.DictionaryContextData();

                // Arrange: Setup the paging logger.
                var pagingLogger = new Mock<IPagingLogger>();
                var pagingLogger2 = new Mock<IPagingLogger>();
                var jobServerQueue = new Mock<IJobServerQueue>();
                jobServerQueue.Setup(x => x.QueueTimelineRecordUpdate(It.IsAny<Guid>(), It.IsAny<TimelineRecord>()));

                hc.EnqueueInstance(pagingLogger.Object);
                hc.EnqueueInstance(pagingLogger2.Object);
                hc.SetSingleton(jobServerQueue.Object);

                var ec = new Runner.Worker.ExecutionContext();
                ec.Initialize(hc);

                // Act.
                ec.InitializeJob(jobRequest, CancellationToken.None);
                ec.Start();

                var embeddedStep = ec.CreateChild(Guid.NewGuid(), "action_1_pre", "action_1_pre", null, null, ActionRunStage.Main, isEmbedded: true);
                embeddedStep.Start();

                embeddedStep.StepTelemetry.Type = "node16";
                embeddedStep.StepTelemetry.Action = "actions/checkout";
                embeddedStep.StepTelemetry.Ref = "v2";

                embeddedStep.AddIssue(new Issue() { Type = IssueType.Error, Message = "error" });
                embeddedStep.AddIssue(new Issue() { Type = IssueType.Warning, Message = "warning" });
                embeddedStep.AddIssue(new Issue() { Type = IssueType.Notice, Message = "notice" });

                embeddedStep.PublishStepTelemetry();

                // Assert.
                Assert.Equal(1, ec.Global.StepsTelemetry.Count);
                Assert.Equal("node16", ec.Global.StepsTelemetry.Single().Type);
                Assert.Equal("actions/checkout", ec.Global.StepsTelemetry.Single().Action);
                Assert.Equal("v2", ec.Global.StepsTelemetry.Single().Ref);
                Assert.Equal(ActionRunStage.Main.ToString(), ec.Global.StepsTelemetry.Single().Stage);
                Assert.True(ec.Global.StepsTelemetry.Single().IsEmbedded);
                Assert.Null(ec.Global.StepsTelemetry.Single().Result);
                Assert.Null(ec.Global.StepsTelemetry.Single().ExecutionTimeInSeconds);
                Assert.Equal(0, ec.Global.StepsTelemetry.Single().ErrorMessages.Count);
            }
        }

        private TestHostContext CreateTestContext([CallerMemberName] String testName = "")
        {
            var hc = new TestHostContext(this, testName);

            // Arrange: Setup the configation store.
            var configurationStore = new Mock<IConfigurationStore>();
            configurationStore.Setup(x => x.GetSettings()).Returns(new RunnerSettings());
            hc.SetSingleton(configurationStore.Object);

            // Arrange: Create the execution context.
            hc.SetSingleton(new Mock<IJobServerQueue>().Object);

            return hc;
        }
    }
}
