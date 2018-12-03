using System;
using System.Collections.Generic;
using System.Linq;
using AppModels.kurumi;

namespace AppModels.Repository {
    public interface IAdorerRepository {
        IEnumerable<TaskOfGroup> GetByGroupId(int groupId);
        IEnumerable<TaskOfGroup> GetAll();
        void Update();
    }
    /// <summary>
    /// TaskOfGroupのRepositoryクラス
    /// </summary>
    public class AdorerRepository : IAdorerRepository, IRepository<TaskOfGroup> {
        kurumiContext _dbContext;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbContext"></param>
        public AdorerRepository(kurumiContext dbContext) {
            this._dbContext = dbContext;
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
        /// タスクの全量を取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskOfGroup> GetAll() {
            using(var db = this._dbContext) {
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
        public void Add(TaskOfGroup item) {
            #region ## TaskGroup／Tasksに分けてAddする ##
            /*
            this._dbContext.Set<TaskOfGroup>().Add(item);
            this._dbContext.SaveChangesAsync();
            */
            #endregion
            throw new NotSupportedException();
        }
        public void Update() => this._dbContext.SaveChangesAsync();
        public void Delete(TaskOfGroup item) {
            throw new NotSupportedException(); // 未定義のメソッド
        }
    }
}