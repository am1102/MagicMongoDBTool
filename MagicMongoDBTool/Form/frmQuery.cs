﻿using MagicMongoDBTool.Module;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MagicMongoDBTool
{
    public partial class frmQuery : System.Windows.Forms.Form
    {
        /// <summary>
        /// 当前DataViewInfo
        /// </summary>
        MongoDBHelper.DataViewInfo CurrentDataViewInfo;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mDataViewInfo">Filter也是DataViewInfo的一个属性，所以这里加上参数</param>
        public frmQuery(MongoDBHelper.DataViewInfo mDataViewInfo)
        {
            InitializeComponent();
            CurrentDataViewInfo = mDataViewInfo;
            SystemManager.SelectObjectTag = mDataViewInfo.strDBTag;
        }
        /// <summary>
        /// 输出配置字典
        /// </summary>
        private void frmQuery_Load(object sender, EventArgs e)
        {
            this.Icon = GetSystemIcon.ConvertImgToIcon(GetResource.GetImage(ImageType.Query));
            List<DataFilter.QueryFieldItem> FieldList = new List<DataFilter.QueryFieldItem>();
            FieldList = CurrentDataViewInfo.mDataFilter.QueryFieldList;
            //增加第一个条件
            ConditionPan.AddCondition();
            if (CurrentDataViewInfo.IsUseFilter)
            {
                //使用过滤：字段和条件的设定
                QueryFieldPicker.setQueryFieldList(FieldList);
                if (CurrentDataViewInfo.mDataFilter.QueryConditionList.Count > 0)
                {
                    ConditionPan.PutQueryToUI(CurrentDataViewInfo.mDataFilter);
                }
            }
            else
            {
                //不使用过滤：字段初始化
                QueryFieldPicker.InitByCurrentCollection(true);
            }
            if (!SystemManager.IsUseDefaultLanguage)
            {
                this.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Query_Title);
                tabFieldInfo.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Query_FieldInfo);
                tabCondition.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Query_Filter);
                tabSql.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.ConvertSql_Title);
                cmdAddCondition.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Query_Filter_AddCondition);
                cmdLoad.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Query_Action_Load);
                cmdSave.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Common_Save);
                cmdOK.Text = SystemManager.mStringResource.GetText(MagicMongoDBTool.Module.StringResource.TextType.Common_OK);
                cmdCancel.Text = SystemManager.mStringResource.GetText(StringResource.TextType.Common_Cancel);
            }
        }
        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddCondition_Click(object sender, EventArgs e)
        {
            ConditionPan.AddCondition();
        }
        /// <summary>
        /// 清空条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ConditionPan.ClearCondition();
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            // 设置DataFilter
            if (string.IsNullOrEmpty(txtSql.Text))
            {
                SetCurrDataFilter();
            }
            else
            {
                CurrentDataViewInfo.mDataFilter = MongoDBHelper.ConvertQuerySql(txtSql.Text);
            }
            ///按下OK，不论是否做更改都认为True
            CurrentDataViewInfo.IsUseFilter = true;
            this.Close();
        }
        /// <summary>
        /// 直接关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = MongoDBHelper.XmlFilter;
            if (savefile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 设置DataFilter
                if (string.IsNullOrEmpty(txtSql.Text))
                {
                    //设置DataFilter
                    SetCurrDataFilter();
                }
                else
                {
                    CurrentDataViewInfo.mDataFilter = MongoDBHelper.ConvertQuerySql(txtSql.Text);
                }
                CurrentDataViewInfo.mDataFilter.SaveFilter(savefile.FileName);
            }
        }
        /// <summary>
        /// 设置DataFilter
        /// </summary>
        private void SetCurrDataFilter()
        {
            //清除以前的结果和内部变量，重要！
            CurrentDataViewInfo.mDataFilter.Clear();
            CurrentDataViewInfo.mDataFilter.DBName = SystemManager.GetCurrentDataBase().Name;
            CurrentDataViewInfo.mDataFilter.CollectionName = SystemManager.GetCurrentCollection().Name;
            CurrentDataViewInfo.mDataFilter.QueryFieldList = QueryFieldPicker.getQueryFieldList();
            ConditionPan.SetCurrDataFilter(CurrentDataViewInfo);

        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = MongoDBHelper.XmlFilter;
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataFilter NewDataFilter = DataFilter.LoadFilter(openFile.FileName);
                CurrentDataViewInfo.mDataFilter = NewDataFilter;
            }
        }
    }
}
