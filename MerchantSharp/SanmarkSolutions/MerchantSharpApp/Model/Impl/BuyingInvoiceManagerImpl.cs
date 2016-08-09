﻿using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Common;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Model.Dao;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Model.Entities;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Utility.Main;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.Utility.UIComponents;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.Modules;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.ProductTransactions;
using MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.ShopManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MerchantSharp.SanmarkSolutions.MerchantSharpApp.Model.Impl {
	class BuyingInvoiceManagerImpl {

		private IDao buyingInvoiceDao = BuyingInvoiceDao.getInstance();
		private IDao buyingItemDao = BuyingItemDao.getInstance();
		private AddBuyingInvoice addBuyingInvoice;
		private BuyingInvoiceHistory buyingInvoiceHistory;

		private ItemManagerImpl itemManagerImpl = null;
		private UnitManagerImpl unitManagerImpl = null;
		private SellingPriceManagerImpl sellingPriceManagerImpl = null;
		private StockManagerImpl stockManagerImpl = null;
		private VendorManagerImpl vendorManagerImpl = new VendorManagerImpl();
		private PaymentManagerImpl paymentManagerImpl = null;
		private UserManagerImpl userManagerImpl = null;
		private BuyingItemHistory buyingItemHistory;
		private CompanyReturnManagerImpl companyReturnManagerImpl = null;


		public BuyingInvoiceManagerImpl() {
			itemManagerImpl = new ItemManagerImpl();
		}

		public BuyingInvoiceManagerImpl(AddBuyingInvoice addBuyingInvoice) {
			this.addBuyingInvoice = addBuyingInvoice;
			itemManagerImpl = new ItemManagerImpl();
			unitManagerImpl = new UnitManagerImpl();
			sellingPriceManagerImpl = new SellingPriceManagerImpl();
			stockManagerImpl = new StockManagerImpl();
			companyReturnManagerImpl = new CompanyReturnManagerImpl();
			paymentManagerImpl = new PaymentManagerImpl();
		}

		public BuyingInvoiceManagerImpl(BuyingInvoiceHistory buyingInvoiceHistory) {
			this.buyingInvoiceHistory = buyingInvoiceHistory;
			this.paymentManagerImpl = new PaymentManagerImpl();
			userManagerImpl = new UserManagerImpl();
		}

		public BuyingInvoiceManagerImpl(BuyingItemHistory buyingItemHistory) {
			this.buyingItemHistory = buyingItemHistory;
		}

		/// <summary>
		/// Ads an Entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <returns>Returns ID of Entity.</returns>
		public int addInvoice(Entity entity) {
			return buyingInvoiceDao.add(entity);
		}

		/// <summary>
		/// Deletes an Entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		/// <returns>Returns true if deleted.</returns>
		public bool delInvoice(Entity entity) {
			return buyingInvoiceDao.del(entity);
		}

		public List<BuyingInvoice> getInvoice(Entity entity) {
			return buyingInvoiceDao.get(entity).Cast<BuyingInvoice>().ToList();
		}

		public int updInvoice(Entity entity) {
			return buyingInvoiceDao.upd(entity);
		}

		/////////////

		public int addItem(Entity entity) {
			return buyingItemDao.add(entity);
		}

		public bool delItem(Entity entity) {
			return buyingItemDao.del(entity);
		}

		public List<BuyingItem> getItem(Entity entity) {
			return buyingItemDao.get(entity).Cast<BuyingItem>().ToList();
		}

		public int updItem(Entity entity) {
			return buyingItemDao.upd(entity);
		}


		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////// Buying Invoice Manager //////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		public BuyingInvoice getInvoiceByGRN(String grn) {
			BuyingInvoice buyingInvoice = null;
			try {
				BuyingInvoice i = new BuyingInvoice();
				i.Grn = grn;
				buyingInvoice = getInvoice(i)[0];
			} catch (Exception) {
			}
			return buyingInvoice;
		}

		public bool isDuplicateGRN(String grn) {
			bool b = false;
			try {
				if (getInvoiceByGRN(grn) != null) {
					b = true;
				}
			} catch (Exception) {
			}
			return b;
		}

		public BuyingItem getBuyingItemById(int id) {
			BuyingItem buyingItem = null;
			try {
				BuyingItem item = new BuyingItem();
				item.Id = id;
				buyingItem = getItem(item)[0];
			} catch (Exception) {
			}
			return buyingItem;
		}

		/// <summary>
		/// Will return next GRN
		/// </summary>
		/// <returns>String number</returns>
		public String getNextGRN() {
			String code = null;
			try {
				BuyingInvoice buyingInvoice = new BuyingInvoice();
				buyingInvoice.OrderBy = "id DESC";
				//buyingInvoice.OrderType = "DESC";
				buyingInvoice.LimitStart = 0;
				buyingInvoice.LimitEnd = 1;
				List<BuyingInvoice> listBuyingInvoice = getInvoice(buyingInvoice);
				if (listBuyingInvoice.Count == 0) {
					buyingInvoice = new BuyingInvoice();
					buyingInvoice.Grn = "0";
				} else {
					buyingInvoice = listBuyingInvoice[listBuyingInvoice.Count - 1];
				}
				bool run = true;
				Int64 intCode = Convert.ToInt64(!String.IsNullOrWhiteSpace(buyingInvoice.Grn) ? buyingInvoice.Grn : "0");
				Int64 intNewCode = intCode + 1;
				while (run) {
					if (intNewCode > 99999999999) {
						run = false;
					} else {
						if (intNewCode > 99999) {
							code = intNewCode.ToString();
						} else if (intNewCode > 9999) {
							code = "0" + intNewCode.ToString();
						} else if (intNewCode > 999) {
							code = "00" + intNewCode.ToString();
						} else if (intNewCode > 99) {
							code = "000" + intNewCode.ToString();
						} else if (intNewCode > 9) {
							code = "0000" + intNewCode.ToString();
						} else {
							code = "00000" + intNewCode.ToString();
						}
					}
					if (!isDuplicateGRN(code)) {
						run = false;
					} else {
						intNewCode++;
					}
				}
			} catch (Exception) {
			}
			return code;
		}

		public bool deleteBuyingItemById(int id) {
			try {
				BuyingItem item = new BuyingItem();
				item.Id = id;
				return delItem(item);
			} catch (Exception) {
				return false;
			}
		}

		internal BuyingInvoice getInvoiceById(int id) {
			BuyingInvoice buyingInvoice = null;
			try {
				BuyingInvoice i = new BuyingInvoice();
				i.Id = id;
				buyingInvoice = getInvoice(i)[0];
			} catch (Exception) {
			}
			return buyingInvoice;
		}

		public List<BuyingItem> getBuyingItemsByInvoiceId(int id) {
			List<BuyingItem> list = null;
			try {
				BuyingItem item = new BuyingItem();
				item.BuyingInvoiceId = id;
				list = getItem(item);
			} catch (Exception) {
			}
			return list;
		}

		public double getSubTotalByInvoiceId(int id) {
			double val = 0;
			try {
				BuyingInvoice invoice = getInvoiceById(id);
				List<BuyingItem> items = getBuyingItemsByInvoiceId(id);
				foreach (BuyingItem buyingItem in items) {
					val += (buyingItem.BuyingPrice * buyingItem.Quantity);
				}
			} catch (Exception) {
			}
			return val;
		}

		public double getNetTotalByInvoiceId(int id) {
			double val = 0;
			try {
				BuyingInvoice invoice = getInvoiceById(id);
				List<BuyingItem> items = getBuyingItemsByInvoiceId(id);
				foreach (BuyingItem buyingItem in items) {
					val += (buyingItem.BuyingPrice * buyingItem.Quantity);
				}
				val -= (invoice.Discount + invoice.MarketReturnDiscount + invoice.LaterDiscount);
			} catch (Exception) {
			}
			return val;
		}

		public int getVendorIdByInvoiceId(int id) {
			try {
				return getInvoiceById(id).VendorId;
			} catch (Exception) {
				return 0;
			}
		}

		public double getLatestBuyingPriceByItemId(int itemId, String mode) {
			double d = 0;
			try {
				BuyingItem bi = new BuyingItem();
				bi.ItemId = itemId;
				bi.OrderBy = "id DESC";
				bi.LimitStart = 0;
				bi.LimitEnd = 1;
				BuyingItem buyingItem = getItem(bi)[0];
				if (mode == buyingItem.BuyingMode) {
					d = buyingItem.BuyingPrice;
				} else if (mode == "p") {
					d = buyingItem.BuyingPrice * itemManagerImpl.getItemById(itemId).QuantityPerPack;
				} else if (mode == "u") {
					d = buyingItem.BuyingPrice / itemManagerImpl.getItemById(itemId).QuantityPerPack;
				}
			} catch (Exception) {
			}
			return d;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////// Add Buying Invoice Implementation //////////////////////////////////
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		private void loadAllItemsForView() {
			try {
				addBuyingInvoice.BuyingInvoice = getInvoiceById(addBuyingInvoice.InvoiceId);
				addBuyingInvoice.textBox_invoiceNumber_basicDetails.Text = addBuyingInvoice.BuyingInvoice.InvoiceNumber;
				if (addBuyingInvoice.BuyingInvoice.Grn != "") {
					addBuyingInvoice.textBox_grnNumber_basicDetails.Text = addBuyingInvoice.BuyingInvoice.Grn;
				}
				addBuyingInvoice.datePicker_date_basicDetails.SelectedDate = addBuyingInvoice.BuyingInvoice.OrderedDate;
				addBuyingInvoice.comboBox_vendor_basicDetails.Value = addBuyingInvoice.BuyingInvoice.VendorId;
				if (addBuyingInvoice.BuyingInvoice.ExpectedPayingDate.ToString("yyyy") != "0001") {
					addBuyingInvoice.datePicker_expectedPayingDate_basicDetails.SelectedDate = addBuyingInvoice.BuyingInvoice.ExpectedPayingDate;
				}
				addBuyingInvoice.textBox_details_basicDetails.Text = addBuyingInvoice.BuyingInvoice.Details;
				addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsChecked = addBuyingInvoice.BuyingInvoice.Status == 2;
				addBuyingInvoice.checkBox_completelyPaid_selectedItems.IsChecked = addBuyingInvoice.BuyingInvoice.IsCompletelyPaid == 1;
				addBuyingInvoice.textBox_discount_selectedItems.DoubleValue = addBuyingInvoice.BuyingInvoice.Discount;
				addBuyingInvoice.textBox_companyReturn_selectedItems.DoubleValue = addBuyingInvoice.BuyingInvoice.MarketReturnDiscount;
				addBuyingInvoice.textBox_laterDiscount_selectedItems.DoubleValue = addBuyingInvoice.BuyingInvoice.LaterDiscount;

				loadOldAllItemToDataTable();
				addBuyingInvoice.PaymentSection.InvoiceId = addBuyingInvoice.InvoiceId;
				addBuyingInvoice.PaymentSection.label_balance_vendorAccountSettlement.Content = vendorManagerImpl.getAccountBalanceById(addBuyingInvoice.comboBox_vendor_basicDetails.Value).ToString("#,##0.00");

				if (addBuyingInvoice.BuyingInvoice.Status == 1) {
					addBuyingInvoice.button_add_selectItem.IsEnabled = false;
					//addBuyingInvoice.dataGrid_selectedItems_selectedItems.IsEnabled = false;
					addBuyingInvoice.textBox_discount_selectedItems.IsReadOnly = true;
					addBuyingInvoice.textBox_companyReturn_selectedItems.IsReadOnly = true;
					addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsEnabled = false;
					addBuyingInvoice.button_return_selectedItems.IsEnabled = true;
				}

				addBuyingInvoice.textBox_remainder_selectedItems.DoubleValue = addBuyingInvoice.textBox_netTotal_selectedItems.DoubleValue - paymentManagerImpl.getAllBuyingPaidAmountForInvoice(addBuyingInvoice.BuyingInvoice.Id) - addBuyingInvoice.textBox_laterDiscount_selectedItems.DoubleValue;
			} catch (Exception) {
			}
		}

		private void loadOldAllItemToDataTable() {
			try {
				List<BuyingItem> list = getBuyingItemsByInvoiceId(addBuyingInvoice.BuyingInvoice.Id);
				addBuyingInvoice.SelectedItems.Rows.Clear();
				foreach (BuyingItem buyingItem in list) {
					DataRow dr = addBuyingInvoice.SelectedItems.NewRow();
					dr[1] = buyingItem.Id.ToString();
					dr[2] = buyingItem.ItemId;
					dr[3] = itemManagerImpl.getItemNameById(buyingItem.ItemId);
					dr[4] = buyingItem.BuyingMode == "p" ? "Pack" : "Unit";
					dr[5] = buyingItem.UnitSellingPrice.ToString("#,##0.00");
					dr[6] = buyingItem.PackSellingPrice.ToString("#,##0.00");
					dr[7] = buyingItem.BuyingPrice.ToString("#,##0.00");
					dr[8] = buyingItem.Quantity.ToString("#,##0.00");
					dr[9] = buyingItem.FreeQuantity.ToString("#,##0.00");
					dr[10] = (buyingItem.BuyingPrice * buyingItem.Quantity).ToString("#,##0.00");
					dr[11] = buyingItem.StockLocationId.ToString();
					addBuyingInvoice.SelectedItems.Rows.Add(dr);
				}
				calculateSubTotal();
				calculateNetTotal();
				setItemCount();
			} catch (Exception) {
			}
		}

		internal void addBuyingInvoiceLoaded() {
			try {
				addBuyingInvoice.SelectedItems = new DataTable();
				addBuyingInvoice.SelectedItems.Columns.Add("#", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("ID", typeof(int));
				addBuyingInvoice.SelectedItems.Columns.Add("ItemId", typeof(int));
				addBuyingInvoice.SelectedItems.Columns.Add("Item", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("Mode", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("USP", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("PSP", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("Price", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("Qty", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("Free Qty", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("Line Total", typeof(String));
				addBuyingInvoice.SelectedItems.Columns.Add("stockId", typeof(int));
				addBuyingInvoice.dataGrid_selectedItems_selectedItems.DataContext = addBuyingInvoice.SelectedItems.DefaultView;

				addBuyingInvoice.dataGrid_selectedItems_selectedItems.Columns[11].Visibility = Visibility.Hidden;
				addBuyingInvoice.dataGrid_selectedItems_selectedItems.Columns[3].MinWidth = 350;

				addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsChecked = addBuyingInvoice.IsRequestOrder;
				addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsEnabled = !addBuyingInvoice.IsRequestOrder;
				addBuyingInvoice.textBox_grnNumber_basicDetails.Text = "Guessed(" + getNextGRN() + ")";
				UIComboBox.vendorsForAddBuyingInvoice(addBuyingInvoice.comboBox_vendor_basicDetails);

				UIComboBox.loadStocks(addBuyingInvoice.comboBox_stock_selectItem, "b");
				addBuyingInvoice.AddSellingPriceUnit = new AddSellingPrice("u");
				addBuyingInvoice.AddSellingPricePack = new AddSellingPrice("p");

				if (Session.Permission["canDeleteSellingPrice"] == 0) {
					addBuyingInvoice.button_sellingPricePerPackDelete_selectItem.IsEnabled = false;
					addBuyingInvoice.button_sellingPricePerUnitDelete_selectItem.IsEnabled = false;
				}
				if (Session.Meta["isActiveExpiryDate"] == 0) {
					addBuyingInvoice.label_expiryDate_selectItem.Visibility = Visibility.Hidden;
					addBuyingInvoice.datePicker_expiryDate_selectItem.Visibility = Visibility.Hidden;
				}
				if (Session.Meta["isActiveMultipleStocks"] == 0) {
					addBuyingInvoice.label_stock_selectItem.Visibility = Visibility.Hidden;
					addBuyingInvoice.comboBox_stock_selectItem.Visibility = Visibility.Hidden;
				}
				if (Session.Meta["isActiveCompanyReturnManager"] == 0) {
					addBuyingInvoice.button_return_selectedItems.Visibility = Visibility.Hidden;
					addBuyingInvoice.label_returnByItems.Visibility = Visibility.Hidden;
					addBuyingInvoice.textBox_returnByItems_selectedItems.Visibility = Visibility.Hidden;
				}
				/*if(addBuyingInvoice.ItemFinder == null) {
					addBuyingInvoice.ItemFinder = new ItemFinder(addBuyingInvoice.textBox_itemId_selectItem);
					addBuyingInvoice.grid_itemFinder.Children.Add(addBuyingInvoice.ItemFinder);
				}*/
				if (addBuyingInvoice.ItemSearch == null) {
					addBuyingInvoice.ItemSearch = new ItemSearch(addBuyingInvoice.textBox_item_selectItem, addBuyingInvoice.textBox_itemId_selectItem);
				}
				if (addBuyingInvoice.PaymentSection == null) {
					addBuyingInvoice.PaymentSection = new PaymentSection("BuyingInvoice");
					addBuyingInvoice.grid_paymentSection.Children.Add(addBuyingInvoice.PaymentSection);
				}
				addBuyingInvoice.button_return_selectedItems.IsEnabled = false;
				if (addBuyingInvoice.IsInvoiceUpdateMode) {
					loadAllItemsForView();
				}
				addBuyingInvoice.textBox_invoiceNumber_basicDetails.Focus();
			} catch (Exception) {
			}
		}

		/// <summary>
		/// Fill item id text box if item is found
		/// </summary>
		internal void selectItemByCode() {
			try {
				String code = addBuyingInvoice.textBox_code_selectItem.Text;
				int id = 0;
				Item item = itemManagerImpl.getItemByBarcode(code);
				if (item != null) {
					id = item.Id;
				} else {
					id = itemManagerImpl.getItemIdByCode(code);
				}
				addBuyingInvoice.textBox_itemId_selectItem.Text = id.ToString();
			} catch (Exception) {
			}
		}

		/// <summary>
		/// Will populate add item form when selec item.
		/// </summary>
		private void populateAddItemForm() {
			try {
				if (addBuyingInvoice.SelectedItem != null) {
					addBuyingInvoice.label_itemName_selectItem.Content = addBuyingInvoice.SelectedItem.Name + "[" + addBuyingInvoice.SelectedItem.Code + "]";
					addBuyingInvoice.radioButton_unit_buyingMode.IsChecked = addBuyingInvoice.SelectedItem.DefaultBuyingMode == "u" ? true : false;
					addBuyingInvoice.radioButton_pack_buyingMode.IsChecked = addBuyingInvoice.SelectedItem.DefaultBuyingMode == "p" ? true : false;
					//addBuyingInvoice.radioButton_unit_buyingMode.Content = "Unit (" + (addBuyingInvoice.SelectedItem.Sip == 0 ? unitManagerImpl.getUnitNameById(addBuyingInvoice.SelectedItem.UnitId) : "") + ")";
					addBuyingInvoice.radioButton_unit_buyingMode.Content = "Unit (" + unitManagerImpl.getUnitNameById(addBuyingInvoice.SelectedItem.UnitId) + ")";
					addBuyingInvoice.radioButton_pack_buyingMode.Content = "Pack (" + (addBuyingInvoice.SelectedItem.Sip == 1 ? addBuyingInvoice.SelectedItem.PackName : unitManagerImpl.getUnitNameById(1)) + ")";
					addBuyingInvoice.radioButton_pack_buyingMode.IsEnabled = addBuyingInvoice.SelectedItem.Sip == 1 ? true : false;

					UIComboBox.sellingPriceForItemAndMode(addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem, addBuyingInvoice.SelectedItem.Id, "u", addBuyingInvoice.AddSellingPriceUnit);
					UIComboBox.sellingPriceForItemAndMode(addBuyingInvoice.comboBox_sellingPricePerPack_selectItem, addBuyingInvoice.SelectedItem.Id, "p", addBuyingInvoice.AddSellingPricePack);

					addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.IsEnabled = addBuyingInvoice.SelectedItem.Sip == 1 ? true : false;
					addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue = getLatestBuyingPriceByItemId(addBuyingInvoice.SelectedItem.Id, addBuyingInvoice.radioButton_pack_buyingMode.IsChecked == true ? "p" : "u");
					if (addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue == 0) {
						addBuyingInvoice.textBox_buyingPrice_selectItem.Focus();
					} else {
						addBuyingInvoice.textBox_buyingQuantity_selectItem.Focus();
					}
				} else {
					addBuyingInvoice.label_itemName_selectItem.Content = null;
				}
			} catch (Exception) {
			}
		}

		/// <summary>
		/// Will select item by item id.
		/// </summary>
		internal void selectItemById() {
			try {
				Item item = itemManagerImpl.getItemById(Convert.ToInt32(addBuyingInvoice.textBox_itemId_selectItem.Text));
				if (item != null) {
					addBuyingInvoice.SelectedItem = item;
					populateAddItemForm();
				} else {
					addBuyingInvoice.SelectedItem = null;
					populateAddItemForm();
					ShowMessage.error(Common.Messages.Error.Error004);
				}
			} catch (Exception) {
			}
		}
		internal bool saveBuyingInvoice(int status) {
			bool b = false;
			try {
				if (Convert.ToInt32(addBuyingInvoice.comboBox_vendor_basicDetails.SelectedValue) < 1) {
					ShowMessage.error(Common.Messages.Error.Error005);
				} else if (addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsChecked == false && addBuyingInvoice.textBox_invoiceNumber_basicDetails.IsNull()) {
					ShowMessage.error(Common.Messages.Error.Error005);
				} else if (status == 1 && addBuyingInvoice.SelectedItems.Rows.Count == 0) {
					ShowMessage.error(Common.Messages.Error.Error012);
				} else {
					BuyingInvoice buyingInvoice = null;
					if (addBuyingInvoice.BuyingInvoice == null) {
						buyingInvoice = new BuyingInvoice();
					} else {
						buyingInvoice = addBuyingInvoice.BuyingInvoice;
					}
					bool isNew = (buyingInvoice.Status != 1 && status == 1) ? true : false;
					buyingInvoice.VendorId = Convert.ToInt32(addBuyingInvoice.comboBox_vendor_basicDetails.SelectedValue);
					buyingInvoice.InvoiceNumber = addBuyingInvoice.textBox_invoiceNumber_basicDetails.TrimedText;
					buyingInvoice.OrderedDate = addBuyingInvoice.datePicker_date_basicDetails.SelectedValue;
					buyingInvoice.Discount = addBuyingInvoice.textBox_discount_selectedItems.DoubleValue;
					buyingInvoice.IsCompletelyPaid = addBuyingInvoice.checkBox_completelyPaid_selectedItems.IsChecked == true ? 1 : 0;
					buyingInvoice.MarketReturnDiscount = addBuyingInvoice.textBox_companyReturn_selectedItems.DoubleValue;
					buyingInvoice.ExpectedPayingDate = addBuyingInvoice.datePicker_expectedPayingDate_basicDetails.SelectedValue;
					buyingInvoice.Details = addBuyingInvoice.textBox_details_basicDetails.TrimedText;
					buyingInvoice.VendorAccountBalanceChange = 0;
					buyingInvoice.VendorAccountBalanceType = 0;
					buyingInvoice.LaterDiscount = addBuyingInvoice.textBox_laterDiscount_selectedItems.DoubleValue;
					buyingInvoice.Status = status;
					int invoiceId = 0;
					if (buyingInvoice.Id > 0) {
						CommonMethods.setCDMDForUpdate(buyingInvoice);
						if (status == 1) {
							if (buyingInvoice.Grn == "" || buyingInvoice.Grn == null) {
								buyingInvoice.Grn = getNextGRN();
							}
							updInvoice(buyingInvoice);
							if (isNew) {
								saveAllBuyingItems();
							}
							addBuyingInvoice.button_return_selectedItems.IsEnabled = true;
						} else {
							updInvoice(buyingInvoice);
						}
					} else {
						buyingInvoice.Grn = "";
						CommonMethods.setCDMDForAdd(buyingInvoice);
						invoiceId = addInvoice(buyingInvoice);
						buyingInvoice.Id = invoiceId;
						addBuyingInvoice.BuyingInvoice = buyingInvoice;
					}
					if (status == 1) {
						addBuyingInvoice.button_add_selectItem.IsEnabled = false;
						//addBuyingInvoice.dataGrid_selectedItems_selectedItems.IsEnabled = false;
						addBuyingInvoice.textBox_discount_selectedItems.IsReadOnly = true;
						addBuyingInvoice.textBox_companyReturn_selectedItems.IsReadOnly = true;
						addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsEnabled = false;
					}
					b = true;
				}
			} catch (Exception) {
			}
			return b;
		}

		private void saveAllBuyingItems() {
			try {
				BuyingItem buyingItem = null;
				StockItem stockItem = null;
				Item item = null;
				foreach (DataRow row in addBuyingInvoice.SelectedItems.Rows) {
					buyingItem = getBuyingItemById(Convert.ToInt32(row["ID"]));
					//buyingItem.BuyingPriceActual = ((Convert.ToDouble(row["Price"]) * Convert.ToDouble(row["Qty"])) / (Convert.ToDouble(row["Qty"]) + Convert.ToDouble(row["Free Qty"]))) - (((addBuyingInvoice.textBox_discount_selectedItems.DoubleValue / Convert.ToDouble(row["Line Total"])) * (buyingItem.BuyingPrice * buyingItem.Quantity)) / (buyingItem.Quantity + buyingItem.FreeQuantity));
					buyingItem.BuyingPriceActual = ((buyingItem.BuyingPrice * buyingItem.Quantity) / (buyingItem.Quantity + buyingItem.FreeQuantity)) - (((addBuyingInvoice.textBox_discount_selectedItems.DoubleValue / Convert.ToDouble(row["Line Total"])) * (buyingItem.BuyingPrice * buyingItem.Quantity)) / (buyingItem.Quantity + buyingItem.FreeQuantity));

					stockItem = stockManagerImpl.getStockItemByStockLocationIdAndItemId(buyingItem.StockLocationId, buyingItem.ItemId);
					item = itemManagerImpl.getItemById(buyingItem.ItemId);

					stockItem.Quantity += ((buyingItem.Quantity + buyingItem.FreeQuantity) * (buyingItem.BuyingMode == "p" ? item.QuantityPerPack : 1));
					stockManagerImpl.updStockItem(stockItem);
					updItem(buyingItem);
					if (buyingItem.BuyingMode == "u") {
						item.UnitBuyingPrice = buyingItem.BuyingPrice;
						item.PackBuyingPrice = buyingItem.BuyingPrice * item.QuantityPerPack;
					} else {
						item.UnitBuyingPrice = buyingItem.BuyingPrice / item.QuantityPerPack;
						item.PackBuyingPrice = buyingItem.BuyingPrice;
					}
					item.UnitSellingPrice = buyingItem.UnitSellingPrice;
					item.PackSellingPrice = buyingItem.PackSellingPrice;
					itemManagerImpl.upd(item);
				}
			} catch (Exception) {
			}
		}

		private bool validateAddItemForm() {
			bool isOkay = true;
			try {
				if (addBuyingInvoice.SelectedItem == null) {
					isOkay = false;
					ShowMessage.error(Common.Messages.Error.Error008);
				} else {
					if (addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.Value <= 0 && addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.IsEnabled) {
						isOkay = false;
						addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.ErrorMode(true);
					}
					if (addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.Value <= 0 && addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.IsEnabled) {
						isOkay = false;
						addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.ErrorMode(true);
					}
					if (addBuyingInvoice.textBox_buyingPrice_selectItem.IsNull()) {
						isOkay = false;
						addBuyingInvoice.textBox_buyingPrice_selectItem.ErrorMode(true);
					}
					if (addBuyingInvoice.textBox_buyingQuantity_selectItem.DoubleValue == 0 && addBuyingInvoice.textBox_buyingQuantityFree_selectItem.DoubleValue == 0) {
						isOkay = false;
						addBuyingInvoice.textBox_buyingQuantity_selectItem.ErrorMode(true);
					}
				}
			} catch (Exception) {
			}
			return isOkay;
		}

		internal void addItemToDataGrid() {
			try {
				if (validateAddItemForm()) {
					DataRow dr = addBuyingInvoice.SelectedItems.NewRow();
					dr[2] = addBuyingInvoice.SelectedItem.Id;
					dr[3] = addBuyingInvoice.SelectedItem.Name;
					dr[4] = addBuyingInvoice.radioButton_pack_buyingMode.IsChecked == true ? "Pack" : "Unit";
					dr[5] = addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.DisplayValue;
					dr[6] = addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.DisplayValue;
					//dr[5] = unitManagerImpl.getUnitNameById(addBuyingInvoice.SelectedItem.UnitId);
					dr[7] = addBuyingInvoice.textBox_buyingPrice_selectItem.FormattedValue;
					dr[8] = addBuyingInvoice.textBox_buyingQuantity_selectItem.FormattedValue;
					dr[9] = addBuyingInvoice.textBox_buyingQuantityFree_selectItem.DoubleValue;
					dr[10] = (addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue * addBuyingInvoice.textBox_buyingQuantity_selectItem.DoubleValue).ToString("#,##0.00");
					dr[11] = Convert.ToInt32(addBuyingInvoice.comboBox_stock_selectItem.SelectedValue);
					BuyingItem buyingItem = new BuyingItem();
					buyingItem.BuyingInvoiceId = addBuyingInvoice.BuyingInvoice.Id;
					buyingItem.ItemId = addBuyingInvoice.SelectedItem.Id;
					buyingItem.StockLocationId = Convert.ToInt32(addBuyingInvoice.comboBox_stock_selectItem.SelectedValue);
					buyingItem.BuyingPrice = addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue;
					// TODO When Save
					buyingItem.BuyingPriceActual = 0;
					buyingItem.UnitSellingPrice = Convert.ToDouble(addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.DoubleValue);
					buyingItem.PackSellingPrice = Convert.ToDouble(addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.DoubleValue);
					buyingItem.Quantity = addBuyingInvoice.textBox_buyingQuantity_selectItem.DoubleValue;
					buyingItem.FreeQuantity = addBuyingInvoice.textBox_buyingQuantityFree_selectItem.DoubleValue;
					buyingItem.BuyingMode = addBuyingInvoice.radioButton_unit_buyingMode.IsChecked == true ? "u" : "p";
					buyingItem.ExpiryDate = (DateTime)addBuyingInvoice.datePicker_expiryDate_selectItem.SelectedDate;
					CommonMethods.setCDMDForAdd(buyingItem);
					dr[1] = addItem(buyingItem);
					addBuyingInvoice.SelectedItems.Rows.Add(dr);
					resetAddItemForm();
					calculateSubTotal();
					calculateNetTotal();
					setItemCount();
					if (Convert.ToInt32(Session.Preference["defaultItemSelectMode"]) == 0) {
						addBuyingInvoice.textBox_item_selectItem.Focus();
					} else {
						addBuyingInvoice.textBox_code_selectItem.Focus();
					}
				}
			} catch (Exception) {
			}
		}

		internal void updItemInDataGrid() {
			try {
				if (validateAddItemForm()) {
					DataRow dr = addBuyingInvoice.SelectedItems.Rows[addBuyingInvoice.UpdateItemSelectedIndex];

					BuyingItem buyingItem = getBuyingItemById(Convert.ToInt32(dr[1]));
					buyingItem.BuyingInvoiceId = addBuyingInvoice.BuyingInvoice.Id;
					//buyingItem.ItemId = addBuyingInvoice.SelectedItem.Id;
					buyingItem.StockLocationId = Convert.ToInt32(addBuyingInvoice.comboBox_stock_selectItem.SelectedValue);
					buyingItem.BuyingPrice = addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue;
					buyingItem.BuyingPriceActual = 0;
					buyingItem.UnitSellingPrice = Convert.ToDouble(addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.DoubleValue);
					buyingItem.PackSellingPrice = Convert.ToDouble(addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.DoubleValue);
					buyingItem.Quantity = addBuyingInvoice.textBox_buyingQuantity_selectItem.DoubleValue;
					buyingItem.FreeQuantity = addBuyingInvoice.textBox_buyingQuantityFree_selectItem.DoubleValue;
					buyingItem.BuyingMode = addBuyingInvoice.radioButton_unit_buyingMode.IsChecked == true ? "u" : "p";
					buyingItem.ExpiryDate = (DateTime)addBuyingInvoice.datePicker_expiryDate_selectItem.SelectedDate;
					CommonMethods.setCDMDForUpdate(buyingItem);
					//dr[0] = addItem(buyingItem);
					updItem(buyingItem);
					//dr[1] = addBuyingInvoice.SelectedItem.Id;
					dr[3] = addBuyingInvoice.SelectedItem.Name;
					dr[4] = addBuyingInvoice.radioButton_pack_buyingMode.IsChecked == true ? "Pack" : "Unit";
					dr[5] = addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.DisplayValue;
					dr[6] = addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.DisplayValue;
					dr[7] = addBuyingInvoice.textBox_buyingPrice_selectItem.FormattedValue;
					dr[8] = addBuyingInvoice.textBox_buyingQuantity_selectItem.FormattedValue;
					dr[9] = addBuyingInvoice.textBox_buyingQuantityFree_selectItem.FormattedValue;
					dr[10] = (addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue * addBuyingInvoice.textBox_buyingQuantity_selectItem.DoubleValue).ToString("#,##0.00");
					dr[11] = Convert.ToInt32(addBuyingInvoice.comboBox_stock_selectItem.SelectedValue);

					resetAddItemForm();
					addBuyingInvoice.IsItemUpdateMode = false;
					//addBuyingInvoice.button_add_selectItem.Content = "Add";
					calculateSubTotal();
					calculateNetTotal();
					if (Convert.ToInt32(Session.Preference["defaultItemSelectMode"]) == 0) {
						addBuyingInvoice.textBox_item_selectItem.Focus();
					} else {
						addBuyingInvoice.textBox_code_selectItem.Focus();
					}
				}
			} catch (Exception) {
			}
		}

		internal bool deleteSellingPrice(String mode) {
			bool b = false;
			try {
				if (mode == "u") {
					b = sellingPriceManagerImpl.deleteSellingPriceById(addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.Value);
					UIComboBox.sellingPriceForItemAndMode(addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem, addBuyingInvoice.SelectedItem.Id, "u", addBuyingInvoice.AddSellingPriceUnit);
				} else {
					b = sellingPriceManagerImpl.deleteSellingPriceById(addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.Value);
					UIComboBox.sellingPriceForItemAndMode(addBuyingInvoice.comboBox_sellingPricePerPack_selectItem, addBuyingInvoice.SelectedItem.Id, "p", addBuyingInvoice.AddSellingPricePack);
				}
			} catch (Exception) {
			}
			return b;
		}

		private void resetAddItemForm() {
			try {
				addBuyingInvoice.textBox_item_selectItem.Clear();
				addBuyingInvoice.textBox_code_selectItem.Clear();
				addBuyingInvoice.radioButton_unit_buyingMode.Content = "";
				addBuyingInvoice.radioButton_pack_buyingMode.Content = "";
				addBuyingInvoice.textBox_buyingQuantity_selectItem.Clear();
				addBuyingInvoice.textBox_buyingQuantityFree_selectItem.Clear();
				addBuyingInvoice.textBox_buyingPrice_selectItem.Clear();
				addBuyingInvoice.textBox_lineTotal_selectItem.Clear();
				addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.SelectedIndex = -1;
				addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.SelectedIndex = -1;
				addBuyingInvoice.datePicker_expiryDate_selectItem.SelectedDate = DateTime.Today;
			} catch (Exception) {
			}
		}

		internal void populateUpdateItemForm() {
			try {
				resetAddItemForm();
				addBuyingInvoice.IsItemUpdateMode = true;
				//addBuyingInvoice.button_add_selectItem.Content = "Update";
				addBuyingInvoice.UpdateItemSelectedIndex = addBuyingInvoice.dataGrid_selectedItems_selectedItems.SelectedIndex;

				DataRow dataRow_items = addBuyingInvoice.SelectedItems.Rows[addBuyingInvoice.UpdateItemSelectedIndex];
				addBuyingInvoice.textBox_itemId_selectItem.Text = dataRow_items["ItemId"].ToString();

				if (Convert.ToString(dataRow_items["Mode"]) == "Pack") {
					addBuyingInvoice.radioButton_pack_buyingMode.IsChecked = true;
				} else {
					addBuyingInvoice.radioButton_unit_buyingMode.IsChecked = true;
				}
				addBuyingInvoice.textBox_buyingQuantity_selectItem.Text = Convert.ToDouble(dataRow_items["Qty"]).ToString();
				addBuyingInvoice.textBox_buyingQuantityFree_selectItem.Text = Convert.ToDouble(dataRow_items["Free Qty"]).ToString();
				addBuyingInvoice.textBox_buyingPrice_selectItem.Text = Convert.ToString(dataRow_items["Price"]);
				addBuyingInvoice.comboBox_sellingPricePerUnit_selectItem.SelectedItem = Convert.ToDouble(dataRow_items["USP"]);
				addBuyingInvoice.comboBox_sellingPricePerPack_selectItem.SelectedItem = Convert.ToDouble(dataRow_items["PSP"]);
				addBuyingInvoice.comboBox_stock_selectItem.SelectedValue = Convert.ToInt32(dataRow_items["stockId"]);
			} catch (Exception) {
			}
		}

		internal void calculateLineTotal() {
			try {
				addBuyingInvoice.textBox_lineTotal_selectItem.DoubleValue = addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue * addBuyingInvoice.textBox_buyingQuantity_selectItem.DoubleValue;
			} catch (Exception) {
			}
		}

		internal void removeSelectedItem() {
			try {
				if (ShowMessage.confirm(MerchantSharpApp.Common.Messages.Information.Info013) == MessageBoxResult.Yes) {
					int index = addBuyingInvoice.dataGrid_selectedItems_selectedItems.SelectedIndex;
					deleteBuyingItemById(addBuyingInvoice.dataGrid_selectedItems_selectedItems.SelectedItemID);
					addBuyingInvoice.SelectedItems.Rows.RemoveAt(index);
					calculateSubTotal();
					calculateNetTotal();
					setItemCount();
					resetAddItemForm();
				}
			} catch (Exception) {
			}
		}

		private void setItemCount() {
			try {
				//addBuyingInvoice.textBox_itemCount_selectedItems.Text = addBuyingInvoice.SelectedItems.Rows.Count.ToString();
				int count = 0;
				foreach (DataRow row in addBuyingInvoice.SelectedItems.Rows) {
					count++;
					row[0] = count.ToString("00");
				}
			} catch (Exception) {
			}
		}

		internal void calculateNetTotal() {
			try {
				double cr = companyReturnManagerImpl.getReturnItemsValueByInvoiceId(addBuyingInvoice.BuyingInvoice.Id);
				addBuyingInvoice.textBox_returnByItems_selectedItems.DoubleValue = cr;
				addBuyingInvoice.textBox_netTotal_selectedItems.DoubleValue = addBuyingInvoice.textBox_subTotal_selectedItems.DoubleValue - addBuyingInvoice.textBox_discount_selectedItems.DoubleValue - addBuyingInvoice.textBox_companyReturn_selectedItems.DoubleValue - cr;
			} catch (Exception) {
			}
		}

		private void calculateSubTotal() {
			try {
				double subTotal = 0;
				foreach (DataRow row in addBuyingInvoice.SelectedItems.Rows) {
					subTotal += Convert.ToDouble(row["Price"]) * Convert.ToDouble(row["Qty"]);
				}
				addBuyingInvoice.textBox_subTotal_selectedItems.DoubleValue = subTotal;
			} catch (Exception) {
			}
		}

		internal void changePriceLabelName() {
			try {
				if (addBuyingInvoice.radioButton_pack_buyingMode.IsChecked == true) {
					addBuyingInvoice.label_buyingPrice_selectItem.Content = "Pack Buying Price (F6)";
					//addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue = getLatestBuyingPriceByItemId(addBuyingInvoice.SelectedItem.Id);
				} else {
					addBuyingInvoice.label_buyingPrice_selectItem.Content = "Unit Buying Price (F6)";
					//addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue = getLatestBuyingPriceByItemId(addBuyingInvoice.SelectedItem.Id);
				}
				addBuyingInvoice.textBox_buyingPrice_selectItem.DoubleValue = getLatestBuyingPriceByItemId(addBuyingInvoice.SelectedItem.Id, addBuyingInvoice.radioButton_pack_buyingMode.IsChecked == true ? "p" : "u");
			} catch (Exception) {
			}
		}

		public void resetBuyingInvoiceUI() {
			try {
				addBuyingInvoice.textBox_invoiceNumber_basicDetails.Clear();
				addBuyingInvoice.textBox_grnNumber_basicDetails.Text = "Guessed(" + getNextGRN() + ")";
				addBuyingInvoice.datePicker_date_basicDetails.SelectedDate = DateTime.Today;
				addBuyingInvoice.comboBox_vendor_basicDetails.SelectedIndex = -1;
				addBuyingInvoice.datePicker_expectedPayingDate_basicDetails.SelectedDate = DateTime.Today;
				addBuyingInvoice.textBox_details_basicDetails.Clear();
				addBuyingInvoice.label_itemName_selectItem.Content = "";
				addBuyingInvoice.textBox_itemId_selectItem.Clear();
				resetAddItemForm();
				addBuyingInvoice.SelectedItems.Rows.Clear();
				//addBuyingInvoice.InvoiceId = 0;
				addBuyingInvoice.SelectedItem = null;
				addBuyingInvoice.IsInvoiceUpdateMode = false;
				addBuyingInvoice.BuyingInvoice = null;
				calculateSubTotal();
				calculateNetTotal();
				//addBuyingInvoice.textBox_itemCount_selectedItems.Clear();

				addBuyingInvoice.button_add_selectItem.IsEnabled = true;
				addBuyingInvoice.dataGrid_selectedItems_selectedItems.IsEnabled = true;
				addBuyingInvoice.textBox_discount_selectedItems.IsReadOnly = false;
				addBuyingInvoice.textBox_companyReturn_selectedItems.IsReadOnly = false;
				addBuyingInvoice.checkBox_isRequestOrder_selectedItems.IsEnabled = true;
			} catch (Exception) {
			}
		}

		internal void loadAccountValueInPaymentSection() {
			try {
				if (addBuyingInvoice.comboBox_vendor_basicDetails.Value > 0) {
					addBuyingInvoice.PaymentSection.label_balance_vendorAccountSettlement.Content = vendorManagerImpl.getAccountBalanceById(addBuyingInvoice.comboBox_vendor_basicDetails.Value).ToString("#,##0.00");
				} else {
					addBuyingInvoice.PaymentSection.label_balance_vendorAccountSettlement.Content = "0.00";
				}
			} catch (Exception) {
			}
		}

		internal void addDiscountManager() {
			try {
				if (Session.Permission["canAccessDiscountManager"] == 1) {
					addBuyingInvoice.DiscountManager = new DiscountManager(addBuyingInvoice.SelectedItem.Id);
					addBuyingInvoice.DiscountManager.mainGrid.Width = 950;
					addBuyingInvoice.DiscountManager.mainGrid.Height = 450;
					Window window = new Window();
					window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
					window.Content = addBuyingInvoice.DiscountManager;
					window.Title = "Discount Manager";
					window.ShowDialog();
				} else {
					ShowMessage.error(Common.Messages.Error.Error010);
				}
			} catch (Exception) {
			}
		}



		///////////////////////////////////////////////////////////////////////////////////////////////////
		// History
		///////////////////////////////////////////////////////////////////////////////////////////////////



		internal void buyingInvoiceHistoryLoaded() {
			try {
				UIComboBox.vendorsForFilter(buyingInvoiceHistory.comboBox_vendor_filter);
				UIComboBox.usersForFilter(buyingInvoiceHistory.comboBox_user_filter);
				UIComboBox.buyingInvoiceStatusForSelect(buyingInvoiceHistory.comboBox_status_filter);
				UIComboBox.yesNoForSelect(buyingInvoiceHistory.comboBox_isCompletelyPaid_filter);
				buyingInvoiceHistory.Pagination = new Pagination();
				buyingInvoiceHistory.Pagination.Filter = buyingInvoiceHistory;
				buyingInvoiceHistory.grid_pagination.Children.Add(buyingInvoiceHistory.Pagination);

				buyingInvoiceHistory.DataTable = new DataTable();
				buyingInvoiceHistory.DataTable.Columns.Add("ID", typeof(int));
				buyingInvoiceHistory.DataTable.Columns.Add("GRN", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Invoice #", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Date", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Sub Total", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Discount", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Company Return", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Net Total", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Total Payments", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Remainder", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Account Balance Change", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("EPD", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Vendor", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("User", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Completely Paid", typeof(String));
				buyingInvoiceHistory.DataTable.Columns.Add("Status", typeof(String));

				buyingInvoiceHistory.DataGridFooter = new DataGridFooter();
				buyingInvoiceHistory.dataGrid_buyingInvoiceHistory.IFooter = buyingInvoiceHistory.DataGridFooter;
				buyingInvoiceHistory.grid_footer.Children.Add(buyingInvoiceHistory.DataGridFooter);
				buyingInvoiceHistory.dataGrid_buyingInvoiceHistory.DataContext = buyingInvoiceHistory.DataTable.DefaultView;
				if (buyingInvoiceHistory.IsRequest) {
					buyingInvoiceHistory.comboBox_status_filter.SelectedValue = 2;
					buyingInvoiceHistory.comboBox_status_filter.IsEnabled = false;
				}
				setRowsCount();
			} catch (Exception) {
			}
		}

		private BuyingInvoice getBuyingInvoiceForFilter() {
			BuyingInvoice buyingInvoice = null;
			try {
				buyingInvoice = new BuyingInvoice();
				buyingInvoice.Grn = buyingInvoiceHistory.textBox_grnNumber_filter.IsNull() ? null : buyingInvoiceHistory.textBox_grnNumber_filter.Text + "%";
				buyingInvoice.InvoiceNumber = buyingInvoiceHistory.textBox_invoiceNumber_filter.IsNull() ? null : "%" + buyingInvoiceHistory.textBox_invoiceNumber_filter.TrimedText + "%";
				buyingInvoice.VendorId = buyingInvoiceHistory.comboBox_vendor_filter.Value;
				buyingInvoice.CreatedBy = buyingInvoiceHistory.comboBox_user_filter.Value;
				if (buyingInvoiceHistory.datePicker_from_filter.SelectedDate != null || buyingInvoiceHistory.datePicker_to_filter.SelectedDate != null) {
					if (buyingInvoiceHistory.datePicker_from_filter.SelectedDate != null && buyingInvoiceHistory.datePicker_to_filter.SelectedDate != null) {
						buyingInvoice.OrderedDate = buyingInvoiceHistory.datePicker_from_filter.SelectedValue;
						buyingInvoice.addDateCondition("ordered_date", "BETWEEN", buyingInvoiceHistory.datePicker_from_filter.SelectedValue.ToString("yyyy-MM-dd"), buyingInvoiceHistory.datePicker_to_filter.SelectedValue.ToString("yyyy-MM-dd"));
					} else if (buyingInvoiceHistory.datePicker_from_filter.SelectedDate != null) {
						buyingInvoice.OrderedDate = buyingInvoiceHistory.datePicker_from_filter.SelectedValue;
					} else {
						buyingInvoice.OrderedDate = buyingInvoiceHistory.datePicker_to_filter.SelectedValue;
					}
				}
				if (buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate != null || buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedDate != null) {
					if (buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate != null && buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedDate != null) {
						buyingInvoice.ExpectedPayingDate = buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedValue;
						buyingInvoice.addDateCondition("ordered_date", "BETWEEN", buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedValue.ToString("yyyy-MM-dd"), buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedValue.ToString("yyyy-MM-dd"));
					} else if (buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate != null) {
						buyingInvoice.ExpectedPayingDate = buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedValue;
					} else {
						buyingInvoice.ExpectedPayingDate = buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedValue;
					}
				}
				buyingInvoice.Status = Convert.ToInt32(buyingInvoiceHistory.comboBox_status_filter.SelectedValue);
				buyingInvoice.IsCompletelyPaid = Convert.ToInt32(buyingInvoiceHistory.comboBox_isCompletelyPaid_filter.SelectedValue);
				buyingInvoice.Details = "%" + buyingInvoiceHistory.textBox_details_filter.TrimedText + "%";
				buyingInvoice.OrderBy = "id DESC";
			} catch (Exception) {
			}
			return buyingInvoice;
		}

		internal void filter() {
			try {
				DataSet dataSet = CommonManagerImpl.getBuyingInvoiceForFilter(buyingInvoiceHistory.textBox_grnNumber_filter.Text,
					buyingInvoiceHistory.textBox_invoiceNumber_filter.Text,
					Convert.ToInt32(buyingInvoiceHistory.comboBox_vendor_filter.SelectedValue),
					Convert.ToInt32(buyingInvoiceHistory.comboBox_user_filter.SelectedValue),
					Convert.ToInt32(buyingInvoiceHistory.comboBox_isCompletelyPaid_filter.SelectedValue),
					Convert.ToInt32(buyingInvoiceHistory.comboBox_status_filter.SelectedValue),
					(buyingInvoiceHistory.datePicker_from_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_from_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingInvoiceHistory.datePicker_to_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_to_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					buyingInvoiceHistory.textBox_details_filter.Text, false, buyingInvoiceHistory.Pagination.LimitStart, buyingInvoiceHistory.Pagination.LimitCount);

				buyingInvoiceHistory.DataTable.Rows.Clear();
				double remainder = 0;
				foreach (DataRow row in dataSet.Tables[0].Rows) {
					try {
						remainder = Convert.ToDouble(row[7]) - Convert.ToDouble(row[8]);
					} catch (Exception) {
						remainder = 0;
					}
					buyingInvoiceHistory.DataTable.Rows.Add(row[0], row[1], row[2], Convert.ToDateTime(row[3]).ToString("yyyy-MM-dd"),
						row[4], row[5], row[6], row[7], row[8], remainder.ToString("#,##0.00"), row[9],
						row[10], row[11], row[12], row[13], row[14]);
				}
			} catch (Exception) {
			}
		}

		internal void setRowsCount() {
			try {
				/*BuyingInvoice invoice = getBuyingInvoiceForFilter();
				invoice.RowsCount = 1;
				List<BuyingInvoice> list = getInvoice(invoice);
				buyingInvoiceHistory.Pagination.RowsCount = list[0].RowsCount;*/
				DataSet dataSet = CommonManagerImpl.getBuyingInvoiceForFilter(buyingInvoiceHistory.textBox_grnNumber_filter.Text,
					buyingInvoiceHistory.textBox_invoiceNumber_filter.Text,
					Convert.ToInt32(buyingInvoiceHistory.comboBox_vendor_filter.SelectedValue),
					Convert.ToInt32(buyingInvoiceHistory.comboBox_user_filter.SelectedValue),
					Convert.ToInt32(buyingInvoiceHistory.comboBox_isCompletelyPaid_filter.SelectedValue),
					Convert.ToInt32(buyingInvoiceHistory.comboBox_status_filter.SelectedValue),
					(buyingInvoiceHistory.datePicker_from_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_from_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingInvoiceHistory.datePicker_to_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_to_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_expectedPayingDateFrom_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedDate != null ? Convert.ToDateTime(buyingInvoiceHistory.datePicker_expectedPayingDateTo_filter.SelectedDate).ToString("yyyy-MM-dd") : null),
					buyingInvoiceHistory.textBox_details_filter.Text, true, buyingInvoiceHistory.Pagination.LimitStart, buyingInvoiceHistory.Pagination.LimitCount);
				buyingInvoiceHistory.Pagination.RowsCount = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
			} catch (Exception) {
			}
		}

		internal void dataGrid_buyingInvoiceHistory_MouseDoubleClick() {
			try {
				if (Session.Permission["canEditBuyingInvoice"] == 1) {
					ThreadPool.openTab(new AddBuyingInvoice(buyingInvoiceHistory.dataGrid_buyingInvoiceHistory.SelectedItemID), "Edit Buying Invoice");
				} else {
					ShowMessage.error(Common.Messages.Error.Error010);
				}
			} catch (Exception) {
			}
		}

		internal void deleteInvoice() {
			try {
				if (Session.Permission["canDeleteBuyingInvoice"] == 1) {
					BuyingInvoice invoice = getInvoiceById(buyingInvoiceHistory.dataGrid_buyingInvoiceHistory.SelectedItemID);
					if (ShowMessage.confirm(Common.Messages.Information.Info013) == MessageBoxResult.Yes && invoice.Status != 1 && delInvoice(invoice)) {
						setRowsCount();
						ShowMessage.success(Common.Messages.Success.Success003);
					}
				} else {
					ShowMessage.error(Common.Messages.Error.Error010);
				}
			} catch (Exception) {
			}
		}


		//////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////


		internal void buyingItemHistoryLoaded() {
			try {
				UIComboBox.vendorsForFilter(buyingItemHistory.comboBox_vendor);
				buyingItemHistory.DataTable = new DataTable();
				buyingItemHistory.DataTable.Columns.Add("ID", typeof(int));
				buyingItemHistory.DataTable.Columns.Add("Item Name", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Vendor", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Invoice #", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("GRN", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Date", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Price", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Unit", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Quantity", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Free Quantity", typeof(String));
				buyingItemHistory.DataTable.Columns.Add("Line Total", typeof(String));

				buyingItemHistory.DataGridFooter = new DataGridFooter();
				buyingItemHistory.dataGrid_buyingItem.IFooter = buyingItemHistory.DataGridFooter;
				buyingItemHistory.grid_footer.Children.Add(buyingItemHistory.DataGridFooter);
				buyingItemHistory.dataGrid_buyingItem.DataContext = buyingItemHistory.DataTable.DefaultView;

				buyingItemHistory.Pagination = new Pagination();
				buyingItemHistory.Pagination.Filter = buyingItemHistory;
				buyingItemHistory.grid_pagination.Children.Add(buyingItemHistory.Pagination);
				setItemsRowsCount();
			} catch (Exception) {
			}
		}

		internal void filterItems() {
			try {
				DataSet dataSet = CommonManagerImpl.getBuyingItemForFilter(buyingItemHistory.textBox_itemName.Text, buyingItemHistory.textBox_itemCode.Text,
					buyingItemHistory.textBox_barcode.Text, Convert.ToInt32(buyingItemHistory.comboBox_vendor.SelectedValue),
					buyingItemHistory.textBox_invoice.Text, buyingItemHistory.textBox_grn.Text,
					(buyingItemHistory.datePicker_from.SelectedDate != null ? Convert.ToDateTime(buyingItemHistory.datePicker_from.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingItemHistory.datePicker_to.SelectedDate != null ? Convert.ToDateTime(buyingItemHistory.datePicker_to.SelectedDate).ToString("yyyy-MM-dd") : null),
					false, buyingItemHistory.Pagination.LimitStart, buyingItemHistory.Pagination.LimitCount);

				buyingItemHistory.DataTable.Rows.Clear();
				foreach (DataRow row in dataSet.Tables[0].Rows) {
					buyingItemHistory.DataTable.Rows.Add(row[0], row[1] + " (" + row[2] + ")", row[3], row[4], row[5], Convert.ToDateTime(row[6]).ToString("yyyy-MM-dd"),
						Convert.ToDouble(row[7]).ToString("#,##0.00"), row[8], Convert.ToDouble(row[9]).ToString("#,##0.00"),
						Convert.ToDouble(row[10]).ToString("#,##0.00"), Convert.ToDouble(row[11]).ToString("#,##0.00"));
				}
			} catch (Exception) {
			}
		}

		internal void setItemsRowsCount() {
			try {
				DataSet dataSet = CommonManagerImpl.getBuyingItemForFilter(buyingItemHistory.textBox_itemName.Text, buyingItemHistory.textBox_itemCode.Text,
					buyingItemHistory.textBox_barcode.Text, Convert.ToInt32(buyingItemHistory.comboBox_vendor.SelectedValue),
					buyingItemHistory.textBox_invoice.Text, buyingItemHistory.textBox_grn.Text,
					(buyingItemHistory.datePicker_from.SelectedDate != null ? Convert.ToDateTime(buyingItemHistory.datePicker_from.SelectedDate).ToString("yyyy-MM-dd") : null),
					(buyingItemHistory.datePicker_to.SelectedDate != null ? Convert.ToDateTime(buyingItemHistory.datePicker_to.SelectedDate).ToString("yyyy-MM-dd") : null),
					true, buyingItemHistory.Pagination.LimitStart, buyingItemHistory.Pagination.LimitCount);
				buyingItemHistory.Pagination.RowsCount = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
			} catch (Exception) {
			}
		}

		public double getActualBuyingPrice(int theItemId, double theStockQuantity, String sellingMode, double sellingQuantity) {
			double returnValue = 0;
			try {
				bool run = true;
				int limit = 0;
				double remainingQuantity = theStockQuantity;
				double tempStock = 0;
				Dictionary<int, double[]> dic = new Dictionary<int, double[]>();
				while (run) {
					BuyingItem buyingItem_request = new BuyingItem();
					buyingItem_request.ItemId = theItemId;
					buyingItem_request.OrderBy = "id DESC";
					buyingItem_request.LimitStart = limit;
					buyingItem_request.LimitEnd = 1;
					List<BuyingItem> list_buyingItem = getItem(buyingItem_request);
					if (list_buyingItem.Count == 0) {
						run = false;
					} else {
						buyingItem_request = list_buyingItem[0];
						Item item = itemManagerImpl.getItemById(buyingItem_request.ItemId);
						double qua = (buyingItem_request.BuyingMode == "u" ? (buyingItem_request.Quantity + buyingItem_request.FreeQuantity) : (buyingItem_request.Quantity + buyingItem_request.FreeQuantity) * item.QuantityPerPack);
						remainingQuantity = remainingQuantity - qua;
						limit++;
						if (remainingQuantity <= 0) {
							tempStock = qua + remainingQuantity;
							run = false;
						} else {
							tempStock = qua;
						}
						if (buyingItem_request.BuyingMode == "p" && sellingMode == "u") {
							dic.Add(limit, new double[] { (buyingItem_request.BuyingPriceActual / item.QuantityPerPack), tempStock });
						} else if (buyingItem_request.BuyingMode == "u" && sellingMode == "p") {
							dic.Add(limit, new double[] { (buyingItem_request.BuyingPriceActual * item.QuantityPerPack), tempStock });
						} else {
							dic.Add(limit, new double[] { (buyingItem_request.BuyingPriceActual), tempStock });
						}
						//dic.Add(limit, new double[] { ( buyingItem_request.BuyingMode == "p" ? buyingItem_request.BuyingPriceActual : ( buyingItem_request.BuyingPriceActual * item.QuantityPerPack ) ), tempStock });
					}
				}
				Item itemReq = itemManagerImpl.getItemById(theItemId);
				int[] arrItems = dic.Keys.ToArray();
				double totalBuyingPrices = 0;
				double totalSellingQuantity = sellingMode == "p" ? sellingQuantity / itemReq.QuantityPerPack : sellingQuantity;
				for (int i = arrItems.Length - 1; i > -1; i--) {
					if (totalSellingQuantity > 0) {
						if (totalSellingQuantity <= dic[arrItems[i]][1]) {
							totalBuyingPrices += (dic[arrItems[i]][0] * totalSellingQuantity);
							totalSellingQuantity -= dic[arrItems[i]][1];
						} else if (totalSellingQuantity > dic[arrItems[i]][1]) {
							totalBuyingPrices += (dic[arrItems[i]][1] * dic[arrItems[i]][0]);
							totalSellingQuantity -= dic[arrItems[i]][1];
						}
					}
				}
				if (totalSellingQuantity > 0) {
					totalBuyingPrices += (dic[arrItems[0]][0] * totalSellingQuantity);
				}
				returnValue = totalBuyingPrices / sellingQuantity;

				if (sellingMode == "p") {
					returnValue *= itemReq.QuantityPerPack;
				}
			} catch (Exception) {
			}
			return returnValue;
		}

		internal void button_return_selectedItems_Click() {
			try {
				AddCompanyReturn addReturn = new AddCompanyReturn(addBuyingInvoice.BuyingInvoice.Id);
				addReturn.ShowDialog();
				calculateNetTotal();
			} catch (Exception) {
			}
		}

		public List<String[]> getStockForPrice(int theItemId, double theStockQuantity) {
			List<String[]> list = new List<String[]>();
			try {
				bool run = true;
				int limit = 0;
				double remainingQuantity = theStockQuantity;
				double tempStock = 0;
				while (run) {
					BuyingItem buyingItem_request = new BuyingItem();
					buyingItem_request.ItemId = theItemId;
					buyingItem_request.OrderBy = "id DESC";
					buyingItem_request.LimitStart = limit;
					buyingItem_request.LimitEnd = 1;
					List<BuyingItem> list_buyingItem = getItem(buyingItem_request);
					if (list_buyingItem.Count == 0) {
						run = false;
					} else {
						buyingItem_request = list_buyingItem[0];
						Item item = itemManagerImpl.getItemById(buyingItem_request.ItemId);
						double qua = (buyingItem_request.BuyingMode == "p" ? ((buyingItem_request.Quantity + buyingItem_request.FreeQuantity) * item.QuantityPerPack) : (buyingItem_request.Quantity + buyingItem_request.FreeQuantity));
						remainingQuantity = remainingQuantity - qua;
						limit++;
						if (remainingQuantity <= 0) {
							tempStock = qua + remainingQuantity;
							run = false;
						} else {
							tempStock = qua;
						}
						//dic.Add(limit, new double[] { buyingItem_request.BuyingPriceActual, tempStock });
						BuyingInvoice buyingInvoice = new BuyingInvoice();
						buyingInvoice.Id = buyingItem_request.BuyingInvoiceId;
						buyingInvoice = getInvoice(buyingInvoice)[0];
						double buyingPrice = buyingItem_request.BuyingMode == "p" ? ((buyingItem_request.BuyingPrice * item.QuantityPerPack) / item.QuantityPerPack) : buyingItem_request.BuyingPrice;
						String[] arr = new String[6];
						arr[0] = itemManagerImpl.getItemNameById(theItemId);
						arr[1] = buyingInvoice.InvoiceNumber;
						arr[2] = Convert.ToDateTime(buyingInvoice.OrderedDate).ToString("yyyy-MM-dd");
						arr[3] = buyingPrice.ToString("#,##0.00");
						arr[4] = buyingItem_request.UnitSellingPrice.ToString("#,##0.00");
						arr[5] = tempStock.ToString("#,##0.00");
						list.Add(arr);
						//dataTable.Rows.Add(itemManagerImpl.getItemNameById(theItemId), buyingInvoice.InvoiceNumber, Convert.ToDateTime(buyingInvoice.Date).ToString("yyyy-MM-dd"), buyingPrice.ToString("#,##0.00"), buyingItem_request.PackSellingPrice.ToString("#,##0.00"), tempStock.ToString("#,##0.00"));
					}
				}
			} catch (Exception) {
			}
			return list;
		}
	}
}
