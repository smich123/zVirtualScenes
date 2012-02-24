Ext.define('zvsMobile.profile.Phone', {
    extend: 'Ext.app.Profile',

    config: {
        views: ['Main']
    },

    isActive: function () {
        //return Ext.os.is.Phone;
        return true;
    },

    launch: function () {
        zvsMobile.tabPanel = Ext.create('zvsMobile.view.phone.Main');

        //see if they are logged in 
        Ext.Ajax.request({
            url: '/API/login',
            method: 'GET',
            params: {
                u: Math.random()
            },
            success: function (response, opts) {
                var result = JSON.parse(response.responseText);

                if (result.success && result.isLoggedIn) {
                    var settings = zvsMobile.tabPanel.items.items[4];
                    settings.items.items[1].fireEvent('loggedIn');
                }
                else {
                    var settings = zvsMobile.tabPanel.items.items[4];
                    zvsMobile.tabPanel.getTabBar().getComponent(0).fireEvent('tap', zvsMobile.tabPanel.getTabBar().getComponent(3));
                    settings.items.items[2].fireEvent('loggedOut');
                }
            },
            failure: function (result, request) {
                var settings = zvsMobile.tabPanel.items.items[4];
                zvsMobile.tabPanel.getTabBar().getComponent(0).fireEvent('tap', zvsMobile.tabPanel.getTabBar().getComponent(3));
                settings.items.items[2].fireEvent('loggedOut');
            }
        });
    }
});