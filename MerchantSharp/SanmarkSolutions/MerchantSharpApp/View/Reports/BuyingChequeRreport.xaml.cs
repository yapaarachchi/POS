﻿using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Controler;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Utility;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.Reports {
	/// <summary>
	/// Interaction logic for BuyingChequeRreport.xaml
	/// </summary>
	public partial class BuyingChequeRreport : UserControl, IFilter {

		private ReportManagerControler reportManagerControler = null;

		private bool isLoadedUI = false;
		public bool IsLoadedUI {
			get { return isLoadedUI; }
			set { isLoadedUI = value; }
		}

		private Pagination pagination = null;
		public Pagination Pagination {
			get { return pagination; }
			set { pagination = value; }
		}

		private DataTable dataTable = null;
		public DataTable DataTable {
			get { return dataTable; }
			set { dataTable = value; }
		}

		private DataGridFooter dataGridFooter = null;
		public DataGridFooter DataGridFooter {
			get { return dataGridFooter; }
			set { dataGridFooter = value; }
		}

		public BuyingChequeRreport() {
			InitializeComponent();
			reportManagerControler = new ReportManagerControler(this);
		}

		public void filter() {
			reportManagerControler.filterBuyingCheque();
		}

		public void setPagination() {
			reportManagerControler.setBuyingChequeRreportRowsCount();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e) {
			reportManagerControler.buyingChequeReport_UserContolLoaded();
		}

		private void button_filter_Click(object sender, RoutedEventArgs e) {
			reportManagerControler.setBuyingChequeRreportRowsCount();
		}
	}
}
