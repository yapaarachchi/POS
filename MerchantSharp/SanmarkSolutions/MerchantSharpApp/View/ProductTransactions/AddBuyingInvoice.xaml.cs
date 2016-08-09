﻿using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Controler;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Model.Entities;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.Modules;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.ShopManagement;
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
	/// Interaction logic for AddBuyingInvoice.xaml
	/// </summary>
	public partial class AddBuyingInvoice : UserControl {

		private BuyingInvoiceManagerControler buyingInvoiceManagerControler = null;
		private bool isRequestOrder = false;
		public bool IsRequestOrder {
			get { return isRequestOrder; }
			set { isRequestOrder = value; }
		}
		private bool isLoadedUI = false;
		public bool IsLoadedUI {
			get { return isLoadedUI; }
			set { isLoadedUI = value; }
		}
		private int invoiceId = 0;
		public int InvoiceId {
			get { return invoiceId; }
			set { invoiceId = value;}
		}

		private Item selectedItem = null;
		internal Item SelectedItem {
			get { return selectedItem; }
			set { selectedItem = value; }
		}

		private bool isInvoiceUpdateMode = false;
		public bool IsInvoiceUpdateMode {
			get { return isInvoiceUpdateMode; }
			set { isInvoiceUpdateMode = value; }
		}
		private BuyingInvoice buyingInvoice = null;
		internal BuyingInvoice BuyingInvoice {
			get { return buyingInvoice; }
			set { buyingInvoice = value; /*paymentSection.InvoiceId = value.Id;*/ }
		}
		private AddSellingPrice addSellingPriceUnit = null;
		public AddSellingPrice AddSellingPriceUnit {
			get { return addSellingPriceUnit; }
			set { addSellingPriceUnit = value; }
		}
		private AddSellingPrice addSellingPricePack = null;
		public AddSellingPrice AddSellingPricePack {
			get { return addSellingPricePack; }
			set { addSellingPricePack = value; }
		}

		/// ******************************* ///

		private bool isItemUpdateMode = false;
		public bool IsItemUpdateMode {
			get { return isItemUpdateMode; }
			set { isItemUpdateMode = value; }
		}

		private int updateItemSelectedIndex = -1;
		public int UpdateItemSelectedIndex {
			get { return updateItemSelectedIndex; }
			set { updateItemSelectedIndex = value; }
		}

		private DataTable selectedItems = null;
		public DataTable SelectedItems {
			get { return selectedItems; }
			set { selectedItems = value; }
		}

		private ItemFinder itemFinder = null;
		public ItemFinder ItemFinder {
			get { return itemFinder; }
			set { itemFinder = value; }
		}

		private PaymentSection paymentSection = null;
		public PaymentSection PaymentSection {
			get { return paymentSection; }
			set { paymentSection = value; }
		}

		private DiscountManager discountManager = null;
		public DiscountManager DiscountManager {
			get { return discountManager; }
			set { discountManager = value; }
		}

		private ItemSearch itemSearch = null;
		public ItemSearch ItemSearch {
			get { return itemSearch; }
			set { itemSearch = value; }
		}
				

		/// ******************************* ///
		

		public AddBuyingInvoice(bool isRequest) {
			InitializeComponent();
			IsRequestOrder = isRequest;
			buyingInvoiceManagerControler = new BuyingInvoiceManagerControler(this);
		}

		public AddBuyingInvoice(int id) {
			InitializeComponent();
			invoiceId = id;
			isInvoiceUpdateMode = true;
			buyingInvoiceManagerControler = new BuyingInvoiceManagerControler(this);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.UserControl_Loaded();
		}

		private void textBox_code_selectItem_KeyDown(object sender, KeyEventArgs e) {
			if(e.Key == Key.Enter) {
				buyingInvoiceManagerControler.textBox_code_selectItem_KeyDown();
			}
		}

		private void textBox_itemId_selectItem_TextChanged(object sender, TextChangedEventArgs e) {
			buyingInvoiceManagerControler.textBox_itemId_selectItem_TextChanged();
		}

		private void UserControl_KeyDown(object sender, KeyEventArgs e) {
			buyingInvoiceManagerControler.UserControl_KeyDown(e);
		}

		private void button_add_selectItem_Click(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.button_add_selectItem_Click();
		}

		private void button_sellingPricePerUnitDelete_selectItem_Click(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.deleteSellingPrice("u");
		}

		private void button_sellingPricePerPackDelete_selectItem_Click(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.deleteSellingPrice("p");
		}

		private void dataGrid_selectedItems_selectedItems_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			buyingInvoiceManagerControler.dataGrid_selectedItems_selectedItems_MouseDoubleClick();
		}

		private void textBox_buyingQuantity_selectItem_TextChanged(object sender, TextChangedEventArgs e) {
			buyingInvoiceManagerControler.calculateLineTotal();
		}

		private void textBox_buyingPrice_selectItem_TextChanged(object sender, TextChangedEventArgs e) {
			try {
				if(textBox_buyingPrice_selectItem.Text.Contains('%')) {
					double price = Convert.ToDouble((radioButton_unit_buyingMode.IsChecked == true) ? comboBox_sellingPricePerUnit_selectItem.DisplayValue : comboBox_sellingPricePerPack_selectItem.DisplayValue);
					double pre = Convert.ToDouble(textBox_buyingPrice_selectItem.Text.Substring(0,textBox_buyingPrice_selectItem.Text.Length-1));
					textBox_buyingPrice_selectItem.DoubleValue = price - ((price * pre) / 100);
					textBox_buyingPrice_selectItem.SelectionStart = textBox_buyingPrice_selectItem.Text.Length;
				} else {
					buyingInvoiceManagerControler.calculateLineTotal();
				}				
			} catch(Exception) {
			}
		}

		private void dataGrid_selectedItems_selectedItems_KeyUp(object sender, KeyEventArgs e) {
			if(e.Key == Key.Delete) {
				buyingInvoiceManagerControler.removeSelectedItem();
			}
		}

		private void textBox_discount_selectedItems_TextChanged(object sender, TextChangedEventArgs e) {
			buyingInvoiceManagerControler.calculateNetTotal();
		}

		private void textBox_companyReturn_selectedItems_TextChanged(object sender, TextChangedEventArgs e) {
			buyingInvoiceManagerControler.calculateNetTotal();
		}

		private void button_saveInvoice_Click(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.button_saveInvoice_Click();
		}

		private void radioButton_unit_buyingMode_Checked(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.buyingModeRadioButtonClicked();
		}

		private void radioButton_pack_buyingMode_Checked(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.buyingModeRadioButtonClicked();
		}

		private void textBox_buyingQuantity_selectItem_KeyDown(object sender, KeyEventArgs e) {
			if(e.Key == Key.Enter) {
				buyingInvoiceManagerControler.button_add_selectItem_Click();
			}
		}

		private void textBox_buyingQuantityFree_selectItem_KeyDown(object sender, KeyEventArgs e) {
			if(e.Key == Key.Enter) {
				buyingInvoiceManagerControler.button_add_selectItem_Click();
			}
		}

		private void textBox_buyingPrice_selectItem_KeyDown(object sender, KeyEventArgs e) {
			if(e.Key == Key.Enter) {
				buyingInvoiceManagerControler.button_add_selectItem_Click();
			}
		}

		private void comboBox_vendor_basicDetails_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			buyingInvoiceManagerControler.comboBox_vendor_basicDetails_SelectionChanged();
		}

		private void button_resetUI_Click(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.button_resetUI_Click();
		}

		private void button_discount_Click(object sender, RoutedEventArgs e) {
			buyingInvoiceManagerControler.button_discount_Click();
		}

		private void button_return_selectedItems_Click( object sender, RoutedEventArgs e ) {
			buyingInvoiceManagerControler.button_return_selectedItems_Click();
		}
	}
}
