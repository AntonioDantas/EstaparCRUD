<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RelatorioManobras.aspx.cs" Inherits="EstaparCRUD.Views.Manobras.RelatorioManobras" %>

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
                   <span class="col-2">CARRO:</span>
                    <div class="col-4">
                      <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCarro" OnInit="ddlCarro_Init">
                      </asp:DropDownList>
                    </div>
                    <span class="col-2">MANOBRISTA:</span>
                    <div class="col-4">
                      <asp:DropDownList runat="server" CssClass="form-control" ID="ddlManobrista" OnInit="ddlManobrista_Init">
                      </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                   <span class="col-2">CLASSIFICAÇÃO:</span>
                    <div class="col-4">
                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlClassificacao" >
                            <asp:ListItem Text="Todos" Value="" Selected="True"></asp:ListItem> 
                            <asp:ListItem Text="Recepção do Veículo" Value="1"></asp:ListItem> 
                            <asp:ListItem Text="Retorno do veículo ao Cliente" Value="0"></asp:ListItem> 
                        </asp:DropDownList>
                    </div>
                    <span class="col-2">DATA:</span>
                    <div class="col-4">
                        <asp:TextBox ID="txtData" runat="server" CssClass="form-control" placeholder="DD/MM/AAAA" onkeyup="formataData(this,event);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <span class="col-2">PERÍODO DE:</span>
                    <div class="col-4">
                        <asp:TextBox ID="txtDe" runat="server" CssClass="form-control" placeholder="DD/MM/AAAA" onkeyup="formataData(this,event);" MaxLength="10"></asp:TextBox>
                    </div>
                    <span class="col-2">PERÍODO ATÉ:</span>
                    <div class="col-4">
                        <asp:TextBox ID="txtAte" runat="server" CssClass="form-control" placeholder="DD/MM/AAAA" onkeyup="formataData(this,event);" MaxLength="10"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group text-center">
                    <asp:LinkButton ID="btnPesquisar" runat="server" CssClass="btn btn-primary btn-preloader" OnClick="btnPesquisar_Click">
                        <span class="glyphicon glyphicon-search"></span> PESQUISAR
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnRelatorio" runat="server" CssClass="btn btn-success btn-preloader" OnClick="btnRelatorio_Click">
                        <span class="glyphicon glyphicon-save"></span> EXCEL
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
                            <asp:BoundField DataField="CarroDaManobra.Placa" SortExpression="Placa" HeaderText="PLACA" ControlStyle-CssClass="text-uppercase" />
                            <asp:BoundField DataField="CarroDaManobra.Modelo.Marca.Marca" SortExpression="Marca" HeaderText="MARCA" ControlStyle-CssClass="text-uppercase" />
                            <asp:BoundField DataField="CarroDaManobra.Modelo.Modelo" SortExpression="Modelo" HeaderText="MODELO" ControlStyle-CssClass="text-uppercase" />
                            <asp:BoundField DataField="ManobristaDaManobra.Nome" SortExpression="Nome" HeaderText="MANOBRISTA" ControlStyle-CssClass="text-uppercase"  />
                            <asp:BoundField DataField="ClassificacaoTexto" SortExpression="ClassificacaoTexto" HeaderText="CLASSIFICAÇÃO" ControlStyle-CssClass="text-uppercase" />
                            <asp:BoundField DataField="DataHora" SortExpression="DataHora" HeaderText="DATA/HORA" ControlStyle-CssClass="text-uppercase" />
                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnAlterar" runat="server" CssClass="btn-warning btn-sm btn-preloader" CommandArgument="<%#((GridViewRow) Container).RowIndex  %>" OnClientClick="return confirm('Atenção! Essa operação pode afetar a consistência dos dados!')" CommandName="alterar">
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
