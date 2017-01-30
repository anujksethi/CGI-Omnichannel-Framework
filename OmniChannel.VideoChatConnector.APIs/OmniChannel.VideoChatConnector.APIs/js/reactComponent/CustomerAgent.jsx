
var CustomerProfile = React.createClass({
    render: function () {
        return (
            <tr class="row" data-id={this.props.customer.diallerKey}>
<td><b>   {this.props.customer.name} </b></td>
                <td><b>   {this.props.customer.diallerKey} </b></td>
                <td>
                <button id="agentButton" data-id={this.props.customer.diallerKey} class="btn btn-primary btn-lg" onClick={this.handleClick}>Pick Customer call</button>
                </td>
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

        var allProfiles = this.props.profileList.map(function (profile) {
            return (
                         <CustomerProfile customer={profile} />
);
        });



        return (
                     <table id="agentTable">
                <tbody>

                    {allProfiles}
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
        var self = this;
        var videoChatHub = $.connection.videoChatHub;
        videoChatHub.client.addCustomerToAgents = function (customer, diallerKey, action) {
            var currentCustomers = self.state.customerArray;
            var index = findByName(currentCustomers, customer);


            if (action == "Remove") {
                //var index = currentCustomers.indexOf(customer);
                if (index > -1)
                { currentCustomers.splice(index, 1); }
            }
            else {

                if (index < 0) {
                    currentCustomers.push({ "name": customer, "diallerKey": diallerKey });
                }
            }


            self.setState({ customerArray: currentCustomers }, function () {
                console.log(this.state.customerArray);
                this.forceUpdate();
            });

        }


        videoChatHub.client.getAllConnectedCustomers = function (customers) {
            var currentCustomers = self.state.customerArray;
            var customerArray = JSON.parse(customers);

            for (var x = 0; x < customerArray.length ; x++)
            {
                currentCustomers.push({ "name": customerArray[x].CustomerName, "diallerKey": customerArray[x].CustomerDiallerKey });
                
            }

        
            
            self.setState({ customerArray: currentCustomers }, function () {
                console.log(this.state.customerArray);
                this.forceUpdate();
            });
          //  alert(customers);
            /*currentCustomers.push({ "name": customer, "diallerKey": diallerKey });
            

            self.setState({ customerArray: currentCustomers }, function () {
                console.log(this.state.customerArray);
                this.forceUpdate();
            });*/

        }



        //var person = prompt("Please enter your name", "Agent Name");
        $('#hidUserName').val("person");
       

        $.connection.hub.logging = true;
        var delay = 5000; //1 second
        setTimeout(function () {

            $.connection.hub.start().done(function () {
                console.log("Connected, transport = " + $.connection.hub.transport.name);
                sendMessage();
            }).fail(function (e) {
                console.log('Connection Error ' + e);
            });

            if (videoChatHub) {
                console.log("react SignalR hub initialized.");
            }
           
        }, delay);
       

        function sendMessage() {
            //if (videoChatHub.server) {
              
                if ($("input[id=hidAgentDiallerKey]").val().length > 0)
                    {
                    videoChatHub.server.connectAgent($('#hidUserName').val(),$("input[id=hidAgentDiallerKey]").val(),  'Order');
                    console.log(" connectAgent from react called");
                   // fetchCustomerList();

                }
            //}
        }
        function fetchCustomerList()
        {

        }
        function findByName(source, customerName) {
            for (var i = 0; i < source.length; i++) {
                if (source[i].name === customerName) {
                    return i;
                }
            }
            console.log("Couldn't find object with id: " + customerName);
            return -1;  
        }
      
    },
    getInitialState: function () {
        return ({
            customerArray: [

            ],

        });
    }

});

ReactDOM.render(<Main />, document.getElementById("customerDiv"));
