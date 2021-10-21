<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaCarro.aspx.cs" Inherits="EstaparCRUD.Views.Carros.ConsultaCarro" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript">

        $(window).load(function () {
            $('#preloader1').delay(0).fadeOut('slow', function () { });
        });
        $(function () {

            $(".btn-preloader").click(function () {
                $('#preloader1').delay(0).fadeIn('fast', function () { });
                $("#genCortina").fadeIn('fast');
            });
        });


   </script>
    <div class="card card-body">
        <div class="form-group row"></div>
        <div class="card card-default">
            <div class="card-header">
                <div class="row">
                    <div class="col-12 text-center">
                        CONSULTA DE CARROS
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <span class="col-md-2">Placa:</span>
                    <div class="col-md-4">
                        <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control text-uppercase" onkeyup="formataPlaca(this,event);"></asp:TextBox>
                    </div>
                    <div class="col-md-6"></div>
                </div>
                <div class="form-group row">
                   <span class="col-2">MARCA:</span>
                    <div class="col-4">
                      <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMarcas" OnInit="ddlMarcas_Init" OnSelectedIndexChanged="ddlMarcas_SelectedIndexChanged" AutoPostBack="true">
                      </asp:DropDownList>
                    </div>
                    <span class="col-2">MODELO:</span>
                    <div class="col-4">
                      <asp:DropDownList runat="server" CssClass="form-control" ID="ddlModelos">
                      </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group text-center">
                    <asp:LinkButton ID="btnPesquisar" runat="server" CssClass="btn btn-primary btn-preloader" OnClick="btnPesquisar_Click">
                        <span class="glyphicon glyphicon-search"></span> PESQUISAR
                    </asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="panel panel-default" id="divResultado" runat="server" visible="false">
            <div class="panel-body">
                <div class="form-group text-center">
                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                </div>
                <div class="form-group text-center">
                    <asp:GridView ID="grdResultado" Width="100%" runat="server" AllowPaging="true" PageSize="20" OnPageIndexChanging="grdResultado_PageIndexChanging" OnRowCommand="grdResultado_RowCommand" HeaderStyle-CssClass="thead-dark" 
                        EmptyDataText="Não foram encontrados dados." CssClass="table table-hover text-uppercase" AllowSorting="true" OnSorting="grdResultado_Sorting" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Placa" SortExpression="Placa" HeaderText="PLACA" ControlStyle-CssClass="text-uppercase" />
                            <asp:BoundField DataField="Modelo.Modelo" SortExpression="Modelo" HeaderText="MODELO" ControlStyle-CssClass="text-uppercase"  />
                            <asp:BoundField DataField="Modelo.Marca.Marca" SortExpression="Modelo.Marca" HeaderText="MARCA" ControlStyle-CssClass="text-uppercase" />
                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CssClass="btn-warning btn-sm btn-preloader" CommandArgument="<%#((GridViewRow) Container).RowIndex  %>" CommandName="alterar">
                                        <span class="glyphicon glyphicon-pencil"></span> ALTERAR
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnExcluir" runat="server" CssClass="btn-danger btn-sm" OnClientClick="return confirm('Deseja realmente excluir esses dados?')" CommandArgument="<%#((GridViewRow) Container).RowIndex  %>" CommandName="excluir">
                                        <span class="glyphicon glyphicon-remove-sign"></span> EXCLUIR
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
