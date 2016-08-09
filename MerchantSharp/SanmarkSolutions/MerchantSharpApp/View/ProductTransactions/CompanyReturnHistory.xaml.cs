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

namespace MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.ProductTransactions {
	/// <summary>
	/// Interaction logic for CompanyReturnHistory.xaml
	/// </summary>
	public partial class CompanyReturnHistory : UserControl, IFilter {

		private CompanyReturnManagerControler companyReturnManagerControler = null;

		private bool isLoadedUI = false;
		public bool IsLoadedUI {
			get { return isLoadedUI; }
			set { isLoadedUI = value; }
		}

		private DataTable dataTable = null;
		public DataTable DataTable {
			get { return dataTable; }
			set { dataTable = value; }
		}

		private Pagination pagination = null;
		public Pagination Pagination {
			get { return pagination; }
			set { pagination = value; }
		}

		private DataGridFooter dataGridFooter = null;
		public DataGridFooter DataGridFooter {
			get { return dataGridFooter; }
			set { dataGridFooter = value; }
		}

		public CompanyReturnHistory() {
			InitializeComponent();
			companyReturnManagerControler = new CompanyReturnManagerControler(this);
		}

		public void filter() {
			companyReturnManagerControler.filterCompanyReturn();
		}

		public void setPagination() {
			companyReturnManagerControler.setCompanyReturnRowsCount();
		}

		private void UserControl_Loaded( object sender, RoutedEventArgs e ) {
			companyReturnManagerControler.CompanyReturnHistoryLoaded();
		}

		private void button_filter_Click( object sender, RoutedEventArgs e ) {
			companyReturnManagerControler.setCompanyReturnRowsCount();
		}
	}
}
