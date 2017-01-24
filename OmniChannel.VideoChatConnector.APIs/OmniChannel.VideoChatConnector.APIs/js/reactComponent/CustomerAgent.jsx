
var CustomerProfile = React.createClass({
    render: function () {
        return (
            <tr class="row" data-id={this.props.customer.diallerKey}>
<td><b>   {this.props.customer.name} </b></td>
                <td><b>   {this.props.customer.diallerKey} </b></td>
                <button id="agentButton" data-id={this.props.customer.diallerKey} class="btn btn-primary btn-lg" onClick={this.handleClick}>Pick Customer call</button>

            </tr>
);
    },
    handleClick: function (e) {
        if (e.target.attributes['data-id'].nodeValue != undefined && e.target.attributes['data-id'].nodeValue.length > 0) {
            console.log(e.target.attributes['data-id'].nodeValue + "button click detected");
            $("input[id=hidCustomerDiallerKey]").val(e.target.attributes['data-id'].nodeValue);
            $("input[id=hidCustomerDiallerKey]").change();
            // alert($("input[id=hidCustomerDiallerKey]").val());
        }
    }
});
var CustomerList = React.createClass({


    render: function () {

        var x = this.props.profileList;

        return (
                     <table id="agentTable">
                <tbody>

                    {this.props.profileList.map(function (profile) {
                       return
                       <CustomerProfile customer={profile } />
                   })
                    }
                </tbody>
                     </table>
            );
    },

});

var Main = React.createClass({

    render: function () {
        return (
            <div>
            <CustomerList profileList={this.state.customerArray}></CustomerList>
            </div>

            );
    },
    componentDidMount: function () {

        var videoChatHub = $.connection.videoChatHub;

        $.connection.hub.logging = true;
        var self = this;
        $.connection.hub.start().done(function () {
            console.log("Connected, transport = " + $.connection.hub.transport.name);
            sendMessage();
        }).fail(function (e) {
            console.log('Connection Error ' + e);
        });

        if (videoChatHub) {
            console.log("react SignalR hub initialized.");
        }

        function sendMessage() {
            if (videoChatHub.server) {
                videoChatHub.server.connectAgent('', '', '');
                console.log(" connectAgent from react called");
            }
        }

        videoChatHub.client.addCustomerToAgents = function (customer, diallerKey) {
            var currentCustomers = self.state.customerArray;
            if (jQuery.inArray(customer, currentCustomers) == -1) {
                currentCustomers.push({ "name": customer, "diallerKey": diallerKey });
            }


            self.setState({ customerArray: currentCustomers });
        };
    },
    getInitialState: function () {
        return ({
            customerArray: [

            ],

        });
    }

});

ReactDOM.render(<Main />, document.getElementById("customerDiv"));
