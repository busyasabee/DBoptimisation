package com.dmitr.romashov;

import java.sql.*;


public class Main {

    public static void connectToBd(){
        long startTime = 0;
        long endTime;
        long elapsedTime;
        int countIterations = 1000;
        int counter = 0;

        try {
            //DriverManager.registerDriver((Driver) Class.forName("org.postgresql.Driver").newInstance());
            Class.forName("org.postgresql.Driver"); // можно без registerDriver
            startTime = System.currentTimeMillis();

                try(Connection connection = DriverManager.getConnection("jdbc:postgresql://localhost:5432/mydb",
                        "postgres", "123");) {
                    while (counter != countIterations){
                        try(Statement st = connection.createStatement();){
                            try(ResultSet rs = st.executeQuery("SELECT version()");){
                                while (rs.next())
                                {
                                    System.out.println(rs.getString(1));
                                }

                            }
                        }
                        ++counter;
                    }

                }

        } catch (Exception e) {
            System.out.println("Exception caught");
            e.printStackTrace();
        }
        endTime = System.currentTimeMillis();
        elapsedTime = endTime - startTime;
        System.out.println(elapsedTime);
        System.out.println(counter);

    }

    public static void main(String[] args) {
        connectToBd();

    }
}
