using System;
using System.Collections.Generic;
using System.Linq;
using AppModels.kurumi;

namespace AppModels.Repository {
    public interface IAdorerRepository {
        IEnumerable<TaskOfGroup> GetByGroupId(int groupId);
    }

    /// <summary>
    /// TaskOfGroupのRepositoryクラス
    /// </summary>
    public class AdorerRepository : IAdorerRepository, IRepository<TaskOfGroup> {
        kurumiContext _context;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context"></param>
        public AdorerRepository(kurumiContext context) {
            this._context = context;
        }

        public TaskOfGroup GetByKey(int id) {
            /* ..明示的に未定義を示す */
            throw new NotSupportedException();
        }

        /// <summary>
        /// グループに紐づいたタスクを取得する
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<TaskOfGroup> GetByGroupId(int groupId) {
            return this.GetAll().Where(x => x.GroupId == groupId);
        }

        /// <summary>
        /// グループならびにタスクの全量を取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskOfGroup> GetAll() {
            using(var db = this._context) {
                // GROUPSを取得する
                var groups = db.TaskGroup;
                // GROUPSとTASKSを結合する
                var query = groups.Join(db.Tasks, x => x.GroupId, y => y.GroupId, (g, t) =>
                    new TaskOfGroup {
                        GroupId = g.GroupId,
                        GroupName = g.Name,
                        TaskId = t.TaskId,
                        Status = t.Status,
                        Content = t.Content,
                        Pic = t.Pic,
                        Period = t.Period
                    });
                // 結果を返却する
                return query.OrderBy(x => x.GroupId).ThenBy(x => x.TaskId).ToList();
            }
        }

        /// <summary>
        /// グループならびにタスクの追加
        /// </summary>
        /// <param name="item"></param>
        public void Add(TaskOfGroup item) {
            // Group追加
            var group = new TaskGroup() {
                GroupId = item.GroupId,
                Name = item.GroupName
            };
            this._context.TaskGroup.Add(group);
            // Task追加
            var task = new Tasks() {
                TaskId = item.TaskId,
                GroupId = item.GroupId,
                Status = item.Status,
                Content = item.Content,
                Pic = item.Pic,
                Period = item.Period
            };
            this._context.Tasks.Add(task);
            this._context.SaveChangesAsync();
        }

        /// <summary>
        /// グループならびにタスクの追加
        /// </summary>
        public void Update() => this._context.SaveChangesAsync();

        public void Delete(TaskOfGroup item) {
            throw new NotSupportedException();
        }
    }
}